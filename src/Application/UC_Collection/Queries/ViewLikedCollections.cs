using Application.Contracts;
using Application.Mappers.CollectionMappers;
using Application.Responses.CollectionResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Queries;

public class ViewLikedCollections
{
    public record Query(int PageSize, int PageIndex)
        : IRequest<Result<PaginatedResponse<AllCollectionDetailsResponse>>>;

    public class Handler(
        IVitomDbContext context,
        ICacheServices cacheServices,
        CurrentUser currentUser
    ) : IRequestHandler<Query, Result<PaginatedResponse<AllCollectionDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<AllCollectionDetailsResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            string key =
                $"liked-collection-{currentUser.User!.Id}-pagesize{request.PageSize}-pageindex{request.PageIndex}";

            PaginatedResponse<AllCollectionDetailsResponse>? cacheResult =
                await cacheServices.GetAsync<PaginatedResponse<AllCollectionDetailsResponse>>(
                    key,
                    cancellationToken
                );

            if (cacheResult is not null)
                return Result.Success(
                    cacheResult,
                    $"Get liked collections belong to user {currentUser.User.Id} successfully"
                );

            IQueryable<Collection> query = context
                .Collections.AsNoTracking()
                .Include(c => c.User)
                .Where(c => c.LikeCollections.Any(l => l.UserId == currentUser.User!.Id && l.DeletedAt == null)) // Filter by current user's likes
                .Where(c => c.DeletedAt == null);

            int totalPage = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);

            IEnumerable<AllCollectionDetailsResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Where(c => c.DeletedAt == null)
                .Select(c => c.MapToAllCollectionDetailsResponse())
                .ToListAsync(cancellationToken);

            cacheResult = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPage
            );

            await cacheServices.SetAsync(key, cacheResult, cancellationToken);

            return Result.Success(
                cacheResult,
                $"Get liked collections belong to user {currentUser.User.Id} successfully"
            );
        }
    }
}