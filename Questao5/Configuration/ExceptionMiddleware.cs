using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using Questao5.Domain.Validations;
using System;

namespace Questao5.Configuration
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _configuration);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IConfiguration configuration)
        {
            var customError = Configure(context, exception, configuration);
            
            Console.WriteLine(exception.ToString());

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(customError, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            })); return;
        }

        private static ErrorModel Configure(HttpContext context, Exception exception, IConfiguration configuration)
        {
            var customError = new ErrorModel();

            switch (exception)
            {
                case BusinessException _:
                    {
                        var applicationException = exception as BusinessException;
                        customError.ErrorMessage = applicationException.Mensagem;
                        customError.TipoErro = applicationException.Tipo;
                        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    }

                //500 Internal Server Error
                //O servidor encontrou um erro não esperado.
                default:
                    customError.ErrorMessage = "Ocorreu um erro não esperado!";
                    context.Response.StatusCode = 500;
                    break;
            }

            return customError;
        }

    }

}
