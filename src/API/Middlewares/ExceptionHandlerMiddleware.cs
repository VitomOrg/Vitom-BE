using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Middlewares;

public class ExceptionHandlerMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/problem+json";
            ProblemDetails problemDetails = new()
            {
                Title = exception.Message,
                Detail = exception.StackTrace,
                Status = (int)HttpStatusCode.BadRequest,
            };
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
        return false;
    }
}