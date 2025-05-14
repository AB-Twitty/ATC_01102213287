using Evenda.App.Models;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Evenda.App.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case UnauthorizedAccessException _:
                        await HandleExceptionAsync(context, ex, HttpStatusCode.Unauthorized);
                        break;
                    default:
                        await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
                        break;
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
        {
            var response = new BaseResponse
            {
                StatusCode = statusCode,
                Message = exception.Message,
                Errors = new Dictionary<string, IList<string>> { { "Exception", new[] { exception.Message } } }
            };

            var responseContent = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;

            return context.Response.WriteAsync(responseContent);
        }
    }
}
