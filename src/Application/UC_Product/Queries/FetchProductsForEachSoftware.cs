using Application.Contracts;
using Application.Mappers.SoftwareMappers;
using Application.Responses.ProductResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Product.Queries;

public class FetchProductsForEachSoftware
{
    public record Query(
        bool AscByCreatedAt,
        string? Type,
        int PageIndex,
        int PageSize
    ) : IRequest<Result<PaginatedResponse<FetchProductsForEachSoftwareResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<FetchProductsForEachSoftwareResponse>>>
    {
        public async Task<Result<PaginatedResponse<FetchProductsForEachSoftwareResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //set key
            string key = $"products-softwarepage-pazesize-{request.PageSize}-pageindex-{request.PageIndex}-sortascbycreatedat{request.AscByCreatedAt}-type{request.Type}";
            //get cache result
            PaginatedResponse<FetchProductsForEachSoftwareResponse>? cacheResponse = await cacheServices.GetAsync<PaginatedResponse<FetchProductsForEachSoftwareResponse>>(key, cancellationToken);
            if (cacheResponse is not null) return Result.Success(cacheResponse, "Get Products by each software successfully!");

            //query
            IQueryable<Software> query =
                context.Softwares
                    .AsSplitQuery()
                    .Include(p => p.ProductSoftwares).ThenInclude(ps => ps.Product).ThenInclude(p => p.ProductTypes).ThenInclude(p => p.Type)
                    .Include(p => p.ProductSoftwares).ThenInclude(ps => ps.Product).ThenInclude(p => p.ProductSoftwares).ThenInclude(p => p.Software)
                    .Include(p => p.ProductSoftwares).ThenInclude(ps => ps.Product).ThenInclude(p => p.ProductImages)
                    .Include(p => p.ProductSoftwares).ThenInclude(ps => ps.Product).ThenInclude(p => p.ModelMaterials)
                    .Include(p => p.ProductSoftwares).ThenInclude(ps => ps.Product).ThenInclude(p => p.Model)
                    .Where(s => s.DeletedAt == null)
                    .Where(s => request.Type == null || s.ProductSoftwares.Any(ps => ps.Product.ProductTypes.Any(p => p.Type.Name.ToLower().Contains(request.Type.ToLower()))))
                    .Where(s => s.ProductSoftwares.Any(ps => ps.Product.DeletedAt == null));

            int count = await query.CountAsync(cancellationToken);

            IEnumerable<FetchProductsForEachSoftwareResponse> result =
                    await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(s => s.MapToFetchProductsForEachSoftwareResponse(request.AscByCreatedAt))
                    .ToListAsync(cancellationToken);

            PaginatedResponse<FetchProductsForEachSoftwareResponse> response =
                new(
                    Data: result,
                    PageIndex: request.PageIndex,
                    PageSize: request.PageSize,
                    TotalPages: (int)Math.Ceiling(count / (double)request.PageSize)
                );

            //set cache
            await cacheServices.SetAsync(key, response, cancellationToken);
            return Result.Success(response, "Get Products by each software successfully!");
        }
    }
}