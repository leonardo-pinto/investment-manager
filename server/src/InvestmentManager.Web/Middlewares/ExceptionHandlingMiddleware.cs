using InvestmentManager.ApplicationCore.DTO;
using System.Text.Json;

namespace InvestmentManager.Web.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

            }
            catch (Exception)
            {
                var error = new ErrorResponse() { Error = "An unexpected error occurred." };
                string errorJson = JsonSerializer.Serialize(error);

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(errorJson);
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
