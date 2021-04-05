using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Exceptions;
using System;
using System.Net;
using System.Text.Json;

namespace SharedKernel.Extensions
{
    public static class HandlerException
    {
        public static string Handle(HttpContext context, Exception exception)
        {
            switch (exception)
            {
                case BadRequestException badRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    if (badRequestException.Failures != null)
                    {
                        return JsonSerializer.Serialize(new ValidationProblemDetails(badRequestException.Failures));
                    }
                    else
                    {
                        return badRequestException.Message;
                    }

                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return JsonSerializer.Serialize(new ProblemDetails
                    {
                        Detail = notFoundException.Message
                    });

                case UnauthorizedAccessException unauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return JsonSerializer.Serialize(new { message = "Forbidden" });
            }

            return string.Empty;
        }
    }
}
