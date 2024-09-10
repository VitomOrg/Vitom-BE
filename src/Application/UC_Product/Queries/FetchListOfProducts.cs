using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Application.UC_Product.Queries
{
    public class FetchListOfProducts
    {
        public record Query(
            decimal PriceFrom,
            decimal PriceTo,
            bool AscByCreatedAt,
            int PageSize,
            int PageIndex,
            string Type,
            string License
        ) : IRequest<Result<PaginatedResponse<ProductDetailsResponse>>>;

        public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<ProductDetailsResponse>>>
        {
            public async Task<Result<PaginatedResponse<ProductDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                //set key
                string key = $"products-productpage-pagesize{request.PageSize}-pageindex{request.PageIndex}-sortascbycreateddate{request.AscByCreatedAt}-type{request.Type}-license{request.License}-pricefrom{request.PriceFrom}-priceto{request.PriceTo}";
                //get cache response
                PaginatedResponse<ProductDetailsResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<ProductDetailsResponse>>(key, cancellationToken);
                if (cacheResponse is not null) return Result.Success(cacheResponse, "Get products successfully!");
                //query
                IQueryable<Product> query = context.Products
                .AsNoTracking()
                .Include(p => p.LikeProducts)
                .Include(p => p.ProductTypes).ThenInclude(p => p.Type)
                .Include(p => p.ProductSoftwares)
                .Include(p => p.ProductImages)
                .Include(p => p.CustomColors)
                .Where(p => p.DeletedAt == null)
                // .Where(p => p.ProductTypes.Any(pt => EF.Functions.Like(pt.Type.Name, $"%{request.Type}%")))
                .Where(p => p.ProductTypes.Any(pt => pt.Type.Name.ToLower().Contains(request.Type.ToLower())))
                .Where(p => EF.Functions.Like((string)(object)p.License, $"%{request.License}%"))
                // .Where(p => ((string)(object)p.License).ToString().Contains(request.License))
                .Where(p => p.Price >= request.PriceFrom && p.Price < request.PriceTo);
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
                .ToListAsync();
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
                return Result.Success(cacheResponse, "Get products successfully!");
            }
        }
    }
}
