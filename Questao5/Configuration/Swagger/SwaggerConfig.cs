using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Questao5.Configuration.Swagger
{
    public static class SwaggerConfig
    {

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, params (string title, string version)[] docConfigs)
        {
            return services.AddSwaggerGen(c =>
            {
                foreach (var docConfig in docConfigs)
                    c.SwaggerDoc(docConfig.version, new OpenApiInfo { Title = docConfig.title, Version = docConfig.version });

                c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "x-api-key",
                    Description = "Please enter de token from logged user in Ábaris"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference{Type = ReferenceType.SecurityScheme, Id = "ApiKey"}
                        },
                        new string[]{ }
                    }
                });

                c.DocInclusionPredicate((version, desc) =>
                {
                    var versions = desc.CustomAttributes()
                    .Where(attr => attr.GetType().GetProperty("Versions") != null)
                    .SelectMany(attr => (attr.GetType().GetProperty("Versions").GetValue(attr) as object[]).Select(v => v.ToString()));

                    return versions.Any(v => $"v{v}" == $"{version}.0");
                });

                c.OperationFilter<SwaggerJsonIgnore>();
                c.OperationFilter<SwaggerRemoveVersionParameterFilter>();
                c.DocumentFilter<SwaggerReplaceVersionWithExactValueInPathFilter>();


                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, params (string url, string name)[] endpointConifigs)
            => app.UseSwagger().UseSwaggerUI(c =>
            {
                foreach (var endpointConfig in endpointConifigs)
                    c.SwaggerEndpoint(endpointConfig.url, endpointConfig.name);
            });

    }
}
