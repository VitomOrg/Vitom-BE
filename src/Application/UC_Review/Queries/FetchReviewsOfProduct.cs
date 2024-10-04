using Application.Contracts;
using Application.Mappers.RatingMappers;
using Application.Mappers.ReviewMappers;
using Application.Responses.RatingResponse.cs;
using Application.Responses.ReviewResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Review.Queries;

public class FetchReviewsOfProduct
{
    public record Query(
        Guid ProductId,
        int PageSize,
        int PageIndex
    ) : IRequest<Result<PaginatedResponse<RatingWithReviewResponse>>>;
    public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<RatingWithReviewResponse>>>
    {
        public async Task<Result<PaginatedResponse<RatingWithReviewResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // set key
            string key = $"reviews-pagesize{request.PageSize}-pageindex-{request.PageIndex}";
            // get cache result
            PaginatedResponse<RatingWithReviewResponse>? cacheResult = await cacheServices.GetAsync<PaginatedResponse<RatingWithReviewResponse>>(key, cancellationToken);
            if (cacheResult is not null) return Result.Success(cacheResult, "Get reviews Successfully");
            // query
            IQueryable<Review> query =
                context.Reviews
                    .AsNoTracking()
                    .Include(s => s.Product)
                    .Include(s => s.User)
                    .Where(s => s.ProductId.Equals(request.ProductId))
                    .Where(s => s.User.DeletedAt == null)
                    .Where(s => s.Product.DeletedAt == null)
                    .Where(s => s.DeletedAt == null);
            //sort
            query = query.OrderByDescending(s => s.CreatedAt);
            // calculating total pages
            int totalPages = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);
            // get result
            IEnumerable<IGrouping<int, Review>> result =
                await query.Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .GroupBy(r => r.Rating)
                    .ToListAsync(cancellationToken);

            // map to paginated result
            cacheResult = new(
                Data: result.Select(s => s.MapToRatingWithReviewResponse()),
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );
            // set to cache
            await cacheServices.SetAsync(key, cacheResult, cancellationToken);
            // return result
            return Result.Success(cacheResult, "Get reviews successfully !");
        }
    }
}