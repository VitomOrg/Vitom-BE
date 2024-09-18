using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Queries;

public class FetchLikedProductByUserId
{
    public record Query(
        bool AscByCreatedAt,
        int PageIndex,
        int PageSize
    ) : IRequest<Result<PaginatedResponse<ProductDetailsResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices, CurrentUser currentUser) : IRequestHandler<Query, Result<PaginatedResponse<ProductDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<ProductDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if user is null
            if (currentUser.User is null) return Result.Forbidden();
            //set key
            string key = $"user-likedproduct-pageindex{request.PageIndex}-pagesize{request.PageSize}-orderascbycreatat{request.AscByCreatedAt}-userid{currentUser.User!.Id}";
            //get cache response
            PaginatedResponse<ProductDetailsResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<ProductDetailsResponse>>(key, cancellationToken);
            if (cacheResponse is not null) return Result.Success(cacheResponse, "Get liked products successfully!");
            //get data from db
            IQueryable<Product> query = context.Products
                .AsNoTracking()
                .Include(p => p.LikeProducts)
                .Include(p => p.ProductTypes).ThenInclude(p => p.Type)
                .Include(p => p.ProductSoftwares)
                .Include(p => p.ProductImages)
                .Where(p => p.LikeProducts.Any(lp => lp.UserId == currentUser.User!.Id))
                .Where(p => p.DeletedAt == null);

            //sort
            if (request.AscByCreatedAt)
            {
                query = query.OrderBy(p => p.CreatedAt);
            }
            else
            {
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            //calculate total pages
            int totalPages = (int)Math.Ceiling((double)query.Count() / request.PageSize);

            //get result
            IEnumerable<ProductDetailsResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => p.MapToProductDetailsResponse())
                .ToListAsync(cancellationToken);

            PaginatedResponse<ProductDetailsResponse> response = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );

            //set cache
            await cacheServices.SetAsync(key, response, cancellationToken);
            return Result.Success(response, "Get liked products successfully!");
        }
    }
}