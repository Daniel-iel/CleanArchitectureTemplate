using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace RideSharingApp.Api.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "X-Correlation-Id";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(CorrelationIdHeader))
            {
                context.Request.Headers[CorrelationIdHeader] = Guid.NewGuid().ToString();
            }
            context.Response.Headers[CorrelationIdHeader] = context.Request.Headers[CorrelationIdHeader];
            await _next(context);
        }
    }
}
