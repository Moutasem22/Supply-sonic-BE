using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
                CreateHostBuilder(args).Build().Run();
            }
            catch (System.Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                throw;
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

            string logFilePath = configuration["LogFilePath:Uri"];
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
                .WriteTo.File(new JsonFormatter(), logFilePath + "/log.json",
                fileSizeLimitBytes: 10_000_000,
                rollOnFileSizeLimit: true,
                shared: true,
                //retainedFileCountLimit:3,
                rollingInterval: RollingInterval.Day,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
                .Enrich.WithProperty("Environment", environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.Now:yyyy-MM}"
            };
        }
    }
}
