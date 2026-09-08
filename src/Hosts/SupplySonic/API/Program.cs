using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
//using Serilog.Sinks.File;
namespace AppAPI
{
    /// <summary>
    /// https://www.humankode.com/asp-net-core/logging-with-elasticsearch-kibana-asp-net-core-and-docker
    /// https://andrewlock.net/writing-logs-to-elasticsearch-with-fluentd-using-serilog-in-asp-net-core/
    /// </summary>
    /// 

    public class Program
    {
        public static void Main(string[] args)
        
        {
            ConfigureLogging();

            //CreateHostBuilder(args).Build().Run();
            //then create the host, so that if the host fails we can log errors
            CreateHost(args);
        }
        private static void CreateHost(string[] args)
        {
            try
            {
                var host = CreateHostBuilder(args).Build();
                ApplyPendingMigrations(host);
                host.Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
            }
        }

        private static void ApplyPendingMigrations(IHost host)
        {
            try
            {
                using var scope = host.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<DBContext>();
                context.Database.Migrate();
            }
            catch (System.Exception ex)
            {
                Log.Warning(ex, "Warning: Failed to apply database migrations, continuing application startup.");
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration(configuration =>
                    {
                        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                        configuration.AddJsonFile(
                            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                            optional: true);
                    })
                    .UseSerilog();
                    //webBuilder.UseSerilog((ctx, config) =>
                    //  {
                    //      config
                    //          .MinimumLevel.Information()
                    //          .Enrich.FromLogContext()                              
                    //          .WriteTo.Elasticsearch();
                    //  });
                    webBuilder.UseStartup<Startup>();
                });

        private static void ConfigureLogging()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true)
                .Build();

            var loggerConfiguration = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console();

            var elasticSinkOptions = ConfigureElasticSink(configuration, environment);
            if (elasticSinkOptions != null)
            {
                try
                {
                    loggerConfiguration.WriteTo.Elasticsearch(elasticSinkOptions);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"Warning: Failed to configure Elasticsearch sink, continuing without it. {ex.Message}");
                }
            }

            try
            {
                string logFilePath = configuration["LogFilePath:Uri"];
                if (string.IsNullOrWhiteSpace(logFilePath))
                {
                    logFilePath = System.IO.Path.Combine(AppContext.BaseDirectory, "Logs");
                }

                loggerConfiguration.WriteTo.File(new JsonFormatter(), System.IO.Path.Combine(logFilePath, "log.json"),
                fileSizeLimitBytes: 10_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                //retainedFileCountLimit:3,
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1));
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Warning: Failed to configure file sink, continuing without it. {ex.Message}");
            }

            Log.Logger = loggerConfiguration
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            var elasticUriValue = configuration["ElasticConfiguration:Uri"];
            if (!Uri.TryCreate(elasticUriValue, UriKind.Absolute, out Uri elasticUri))
            {
                return null;
            }

            try
            {
                return new ElasticsearchSinkOptions(elasticUri)
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.Now:yyyy-MM}"
                };
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Warning: Failed to build Elasticsearch sink options, continuing without it. {ex.Message}");
                return null;
            }
        }
    }
}
