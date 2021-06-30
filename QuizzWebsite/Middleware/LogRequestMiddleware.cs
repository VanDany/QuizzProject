using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuizzWebsite.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class LogRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public LogRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {

            using var buffer = new MemoryStream();
            var request = httpContext.Request;
            var response = httpContext.Response;
            var stream = response.Body;
            response.Body = buffer;
            await _next(httpContext);
            Debug.WriteLine($"Request content type: {httpContext.Request.Headers["Accept"]} {Environment.NewLine}" +
                            $"Request path: { request.Path} { Environment.NewLine}" +
                            $"Response type: {response.ContentType} {Environment.NewLine}" +
                            $"Response length: {response.ContentLength ?? buffer.Length}");
            buffer.Position = 0;
            await buffer.CopyToAsync(stream);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseLogRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LogRequestMiddleware>();
        }
    }
}
