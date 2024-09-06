using Application.Contracts;
using Application.Mappers.TypeMappers;
using Application.Responses.Shared;
using Application.Responses.TypeResponses;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Type.Queries;

public class ViewListOfType
{
    public record Query(string Keyword, int PageSize, int PageIndex)
        : IRequest<Result<PaginatedResponse<TypeDetailsResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices)
        : IRequestHandler<Query, Result<PaginatedResponse<TypeDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<TypeDetailsResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            // set key
            string key =
                $"type-keyword{request.Keyword}-pagesize{request.PageSize}-pageindex-{request.PageIndex}";

            PaginatedResponse<TypeDetailsResponse>? cacheResult = await cacheServices.GetAsync<
                PaginatedResponse<TypeDetailsResponse>
            >(key, cancellationToken);

            if (cacheResult is not null)
                return Result.Success(cacheResult, "Get types successfully !");

            IQueryable<Type> query = context
                .Types.AsNoTracking()
                .Where(t =>
                    t.Name.ToLower().Contains(request.Keyword.ToLower()) && t.DeletedAt != null
                );

            int totalPages = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);

            // get result
            IEnumerable<TypeDetailsResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => t.MapToTypeDetailsResponse())
                .ToListAsync(cancellationToken);

            // map to paginated result
            cacheResult = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );

            await cacheServices.SetAsync(key, cacheResult, cancellationToken);

            return Result.Success(cacheResult, "Get types successfully !");
        }
    }
}
