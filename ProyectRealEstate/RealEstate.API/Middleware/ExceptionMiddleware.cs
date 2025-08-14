using System.Net;
using System.Text.Json;

namespace RealEstate.API.Middleware
{

    public class ExceptionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var codeName = "InternalServerError";
            string message = "Ocurrió un error inesperado.";

            if (exception is ArgumentException)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
                codeName = "BadRequest";
                message = exception.Message;
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = (int)HttpStatusCode.NotFound;
                codeName = "NotFound";
                message = exception.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                codeName = "Unauthorized";
                message = exception.Message;
            }

            var response = new
            {
                data = string.Empty,
                code = statusCode,
                codeName,
                message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));

        }
    }
}
