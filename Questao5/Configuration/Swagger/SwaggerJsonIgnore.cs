using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Questao5.Configuration.Swagger
{
    public class SwaggerJsonIgnore : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                .SelectMany(p => p.ParameterType.GetProperties())
                .Where(prop => prop.GetCustomAttributes<JsonIgnoreAttribute>().Any());

            if (ignoredProperties.Any())
                operation.Parameters = operation.Parameters
                    .Where(p => !ignoredProperties.Select(ip => ip.Name.ToLower()).Contains(p.Name.ToLower()))
                    .ToList();
        }
    }
}
