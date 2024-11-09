using Ardalis.Result;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace API.Middlewares;

public class ValidationExceptionHandlerMiddleware : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is ValidationAppException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            httpContext.Response.ContentType = "application/problem+json";
            Result result = Result.Error(new ErrorList(ex.Errors.SelectMany(x => x.Value).ToArray()));
            await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);
            return true;
        }
        return false;
    }
}