using Microsoft.AspNetCore.Http;
using OrdersManagementSystem.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace OrdersManagementSystem.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                await _next(context);
            } catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Set status code based on exception type
            context.Response.StatusCode = exception switch
            {
                ValidationException => (int) HttpStatusCode.BadRequest,
                OrderNotFoundException => (int) HttpStatusCode.NotFound,
                _ => (int) HttpStatusCode.InternalServerError
            };

            var response = new
            {
                error = exception.Message,
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
