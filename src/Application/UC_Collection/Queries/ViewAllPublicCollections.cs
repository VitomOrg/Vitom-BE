using Application.Contracts;
using Application.Mappers.CollectionMappers;
using Application.Responses.CollectionResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Queries;

public class ViewAllPublicCollections
{
    public record Query(int PageSize, int PageIndex)
        : IRequest<Result<PaginatedResponse<AllCollectionDetailsResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices)
        : IRequestHandler<Query, Result<PaginatedResponse<AllCollectionDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<AllCollectionDetailsResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            string key = $"collection-pagesize{request.PageSize}-pageindex{request.PageIndex}";

            PaginatedResponse<AllCollectionDetailsResponse>? cacheResult =
                await cacheServices.GetAsync<PaginatedResponse<AllCollectionDetailsResponse>>(
                    key,
                    cancellationToken
                );

            if (cacheResult is not null)
                return Result.Success(cacheResult, "Get collections successfully");

            IQueryable<Collection> query = context
                .Collections.AsNoTracking()
                .Include(c => c.User)
                .Where(c => c.IsPublic)
                .Where(c => c.DeletedAt == null);

            int totalPages = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);

            IEnumerable<AllCollectionDetailsResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => c.MapToAllCollectionDetailsResponse())
                .ToListAsync(cancellationToken);

            cacheResult = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );

            await cacheServices.SetAsync(key, cacheResult, cancellationToken);

            return Result.Success(cacheResult, "Get all public collections successfully");
        }
    }
}