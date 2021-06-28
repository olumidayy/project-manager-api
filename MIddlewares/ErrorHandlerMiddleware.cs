using Microsoft.AspNetCore.Http;
using ProjectManager.Common;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectManager.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                response.StatusCode = 500;

                var result = JsonSerializer.Serialize(new CustomResponse(status: "error", message: error?.Message));
                await response.WriteAsync(result);
            }
        }
    }
}