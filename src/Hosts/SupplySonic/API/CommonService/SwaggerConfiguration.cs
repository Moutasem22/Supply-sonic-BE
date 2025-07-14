using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AppAPI.CommonService
{
    internal static class SwaggerConfiguration
    {
        internal static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Application API V1", Version = "v1" });
                c.OperationFilter<AddRequiredHeaderParameter>();
            });
            return services;
        }
    }
    //For Read More 
    //https://stackoverflow.com/questions/41493130/web-api-how-to-add-a-header-parameter-for-all-api-in-swagger
    internal class SwaggerHeaderAttribute : Attribute
    {
        public string HeaderName { get; }
        public string Description { get; }
        public string DefaultValue { get; }
        public bool IsRequired { get; }

        public SwaggerHeaderAttribute(string headerName, string description = null, string defaultValue = null, bool isRequired = false)
        {
            HeaderName = headerName;
            Description = description;
            DefaultValue = defaultValue;
            IsRequired = isRequired;
        }
    }

    internal class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();



            if (context.MethodInfo.GetCustomAttribute(typeof(SwaggerHeaderAttribute)) is SwaggerHeaderAttribute attribute)
            {
                var existingParam = operation.Parameters.FirstOrDefault(p =>
                    p.In == ParameterLocation.Header && p.Name == attribute.HeaderName);
                if (existingParam != null) // remove description from [FromHeader] argument attribute
                {
                    operation.Parameters.Remove(existingParam);
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = attribute.HeaderName,
                    In = ParameterLocation.Header,
                    Description = attribute.Description,
                    Required = attribute.IsRequired,
                    Schema = string.IsNullOrEmpty(attribute.DefaultValue)
                        ? null
                        : new OpenApiSchema
                        {
                            Type = "String",
                            Default = new OpenApiString(attribute.DefaultValue)
                        }
                });
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "access token",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("Bearer ")
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Lang",
                In = ParameterLocation.Header,
                Description = "Default Langague",
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("ar")
                }
            });

        }
    }

}
