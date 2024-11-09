using Ardalis.Result;
using Microsoft.AspNetCore.Diagnostics;
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
            Result result = Result.CriticalError(ex.Message);
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
            return true;
        }
        return false;
    }
}