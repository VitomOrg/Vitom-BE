using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Product.Queries;

public class ViewDetailOfProduct
{
    public record Query(Guid Id) : IRequest<Result<ProductDetailsResponse>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices, CurrentUser currentUser)
        : IRequestHandler<Query, Result<ProductDetailsResponse>>
    {
        public async Task<Result<ProductDetailsResponse>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            string key = $"product-{request.Id}";

            ProductDetailsResponse? cacheResult =
                await cacheServices.GetAsync<ProductDetailsResponse>(key, cancellationToken);

            if (cacheResult is not null)
                return Result.Success(cacheResult, "Get product successfully !");

            IQueryable<Product> query = context
                .Products.AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.UserLibraries)
                .Include(p => p.LikeProducts)
                .Include(p => p.CollectionProducts)
                .Include(p => p.Model)
                .Include(p => p.ProductTypes).ThenInclude(p => p.Type)
                .Include(p => p.ProductSoftwares).ThenInclude(p => p.Software)
                .Include(p => p.ProductImages)
                .Include(p => p.ModelMaterials)
                .Include(p => p.Reviews)
                .ThenInclude(p => p.User)
                .Where(p => p.DeletedAt == null)
                .Where(p => p.Id == request.Id);

            ProductDetailsResponse? result = await query
                .Select(p => p.MapForProductDetail(currentUser))
                .FirstOrDefaultAsync(cancellationToken);

            if (result is null)
                return Result.NotFound("Product not found !");

            await cacheServices.SetAsync(request.Id.ToString(), result, cancellationToken);

            return Result.Success(result, "Get product successfully !");
        }
    }
}