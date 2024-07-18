using Ardalis.Result;

namespace API.Utils;
public static class ResultCheckStatus
{
    public static Microsoft.AspNetCore.Http.IResult Check<T>(this Result<T> result)
        => result.Status switch
        {
            ResultStatus.Ok => Results.Ok(result),
            ResultStatus.Created => Results.Created(),
            ResultStatus.Error => Results.BadRequest(result),
            ResultStatus.Forbidden => Results.Forbid(),
            ResultStatus.Unauthorized => Results.Unauthorized(),
            ResultStatus.Invalid => Results.BadRequest(result),
            ResultStatus.NotFound => Results.NotFound(result),
            ResultStatus.NoContent => Results.NoContent(),
            ResultStatus.Conflict => Results.Conflict(result),
            ResultStatus.CriticalError => Results.BadRequest(result),
            ResultStatus.Unavailable => Results.StatusCode(503),
            _ => Results.StatusCode(503),
        };
}