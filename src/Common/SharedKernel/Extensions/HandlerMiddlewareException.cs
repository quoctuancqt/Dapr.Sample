using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedKernel.Extensions
{
    public class HandlerMiddlewareException
    {
        private readonly RequestDelegate _next;


        private readonly ILogger _logger;

        private readonly Func<HttpContext, Exception, string> _action;

        public HandlerMiddlewareException(RequestDelegate next,
            ILoggerFactory loggerFactory,
            Func<HttpContext, Exception, string> action = null)
        {
            _next = next;

            _logger = loggerFactory
                 .CreateLogger<HandlerMiddlewareException>();

            _action = action;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);

                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = string.Empty;

            if (_action != null)
            {
                result = _action.Invoke(context, exception);
            }

            if (string.IsNullOrEmpty(result))
            {
                _logger.LogError(CreateErrorMessage(exception));

                result = JsonSerializer.Serialize(new { message = "Something went wrong." });
            }

            return context.Response.WriteAsync(result);

        }

        private string CreateErrorMessage(Exception ex)
        {
            var message = $"Exception caught in global error handler, exception message: {ex.Message}, exception stack: {ex.StackTrace}";

            if (ex.InnerException != null)
            {
                message = $"{message}, inner exception message {ex.InnerException.Message}, inner exception stack {ex.InnerException.StackTrace}";
            }

            return message;
        }
    }

    public static class HandlerMiddlewareExceptionExtenstion
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder,
            Func<HttpContext, Exception, string> action = null)
        {
            if (action == null)
            {
                return builder.UseMiddleware<HandlerMiddlewareException>();
            }
            else
            {
                return builder.UseMiddleware<HandlerMiddlewareException>(action);
            }
        }
    }
}
