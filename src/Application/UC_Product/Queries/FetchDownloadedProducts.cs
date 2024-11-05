using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Product.Queries;

public class FetchDownloadedProducts
{
    public record Query(
        int PageSize,
        int PageIndex,
        bool AscByCreatedAt
    ) : IRequest<Result<PaginatedResponse<ProductDetailsResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices, CurrentUser currentUser) : IRequestHandler<Query, Result<PaginatedResponse<ProductDetailsResponse>>>
    {
        public async Task<Result<PaginatedResponse<ProductDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if current user is not authenticated
            if (currentUser.User is null || currentUser.User.DeletedAt != null)
                return Result.Forbidden();

            //set key
            string key = $"products-downloaded-productpage-pagesize{request.PageSize}-pageindex{request.PageIndex}-sortascbycreateddate{request.AscByCreatedAt}";
            PaginatedResponse<ProductDetailsResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<ProductDetailsResponse>>(key, cancellationToken);
            if (cacheResponse is not null) return Result.Success(cacheResponse, "Get downloaded product successfully");

            //Query
            IQueryable<Product> query = context.Products
                .AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.ProductTypes).ThenInclude(p => p.Type)
                .Include(p => p.ProductSoftwares).ThenInclude(p => p.Software)
                .Include(p => p.ProductImages)
                .Include(p => p.ModelMaterials)
                .Include(p => p.Model)
                .Where(p => p.DeletedAt == null)
                .Where(p => p.UserLibraries.Any(x => x.UserId == currentUser.User.Id));

            //Sort
            if (request.AscByCreatedAt)
            {
                query = query.OrderBy(x => x.CreatedAt);
            }
            else
            {
                query = query.OrderByDescending(x => x.CreatedAt);
            }

            //calculate total pages
            int totalPages = (int)Math.Ceiling((double)query.Count() / request.PageSize);

            //get result
            IEnumerable<ProductDetailsResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(p => p.MapToProductDetailsResponse())
                .ToListAsync(cancellationToken);

            //map to paginated response
            cacheResponse = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );

            //set to cache
            await cacheServices.SetAsync(key, cacheResponse, cancellationToken);

            //return result
            return Result.Success(cacheResponse, "Get downloaded product successfully");
        }
    }
}