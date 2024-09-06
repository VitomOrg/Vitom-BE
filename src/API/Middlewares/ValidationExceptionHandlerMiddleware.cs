using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
            await httpContext.Response.WriteAsJsonAsync(ex.Errors, cancellationToken);
            return true;
        }
        return false;
    }
}