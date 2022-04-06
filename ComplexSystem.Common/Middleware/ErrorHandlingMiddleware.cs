using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ComplexSystem.Common.Middleware
{
    /// <summary>
    /// The error handling middleware.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// The next.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// The logger.
        /// </summary>
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">
        /// The next.
        /// </param>
        /// <param name="loggerFactory">
        /// The logger factory.
        /// </param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// The handle exception async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="exception">
        /// The exception.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string errorTimeStamp = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.ff");

            await WriteErrorAsync(context, exception.Message, HttpStatusCode.InternalServerError);
        }

        /// <summary>
        /// The write error async.
        /// </summary>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <param name="error">
        /// The error.
        /// </param>
        /// <param name="statusCode">
        /// The status code.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        private async Task WriteErrorAsync(HttpContext context, string error, HttpStatusCode statusCode)
        {
            var serializedError = JsonConvert.SerializeObject(new { error });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(serializedError);
        }
    }
}
