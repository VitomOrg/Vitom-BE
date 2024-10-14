using Application.Contracts;
using Application.Mappers.TypeMappers;
using Application.Responses.TypeResponses;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Type.Queries;

public class ViewTypeById
{
    public record Query(Guid Id) : IRequest<Result<TypeDetailsResponse>>;

    public class Hanlder(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<TypeDetailsResponse>>
    {
        public async Task<Result<TypeDetailsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            // set key
            string key = $"type-id-{request.Id}";
            // get cache
            TypeDetailsResponse? cacheResult = await cacheServices.GetAsync<TypeDetailsResponse>(key, cancellationToken);
            if (cacheResult is not null) return Result.Success(cacheResult, "Get type successfully !");
            // check if type exists
            Type? checkingType = await context
                .Types
                .AsNoTracking()
                .Where(t => t.DeletedAt == null)
                .Where(t => t.Id.Equals(request.Id))
                .FirstOrDefaultAsync(cancellationToken);
            // check if type not found
            if (checkingType is null) return Result.NotFound("Type not found !");
            // set cache
            cacheResult = checkingType.MapToTypeDetailsResponse();
            await cacheServices.SetAsync(key, cacheResult, cancellationToken);
            // return result
            return Result.Success(cacheResult);
        }
    }
}