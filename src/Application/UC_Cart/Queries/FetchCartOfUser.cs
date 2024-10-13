using Application.Contracts;
using Application.Mappers.CartMappers;
using Application.Responses.CartResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Cart.Queries;

public class FetchCartOfUser
{
    public record Query(
        bool AscByCreatedAt = false,
        int PageIndex = 1,
        int PageSize = 10
    ) : IRequest<Result<PaginatedResponse<CartItemResponse>>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<CartItemResponse>>>
    {
        public async Task<Result<PaginatedResponse<CartItemResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if user is null
            if (currentUser.User is null || currentUser.User.DeletedAt != null) return Result.Forbidden();
            //set key
            string key = $"user-cart-pageindex{request.PageIndex}-pagesize{request.PageSize}-orderascbycreatat{request.AscByCreatedAt}-userid{currentUser.User!.Id}";
            //get cache result
            PaginatedResponse<CartItemResponse>? cacheResult = await cacheServices.GetAsync<PaginatedResponse<CartItemResponse>>(key, cancellationToken);
            if (cacheResult is not null) return Result.Success(cacheResult, "Get cart successfully");
            //get data from db
            IQueryable<CartItem> items = context.CartItems
                .AsNoTracking()
                .AsSplitQuery()
                .Include(ci => ci.Product).ThenInclude(ci => ci.ProductImages)
                .Include(ci => ci.Product).ThenInclude(ci => ci.ModelMaterials)
                .Include(ci => ci.Product).ThenInclude(ci => ci.Model)
                .Include(ci => ci.Product).ThenInclude(ci => ci.ProductTypes).ThenInclude(ci => ci.Type)
                .Include(ci => ci.Product).ThenInclude(ci => ci.ProductSoftwares).ThenInclude(ci => ci.Software)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == currentUser.User!.Id)
                .Where(ci => ci.Cart.DeletedAt == null)
                .Where(ci => ci.DeletedAt == null)
                .Where(ci => ci.Product.DeletedAt == null);
            //sort
            if (request.AscByCreatedAt)
            {
                items = items.OrderBy(ci => ci.CreatedAt);
            }
            else
            {
                items = items.OrderByDescending(ci => ci.CreatedAt);
            }
            //calculate total pages
            int totalPages = (int)Math.Ceiling((double)items.Count() / request.PageSize);
            //get results
            IEnumerable<CartItemResponse> result = await items
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(ci => ci.MapToCartItemResponse())
                .ToListAsync(cancellationToken);
            //map to paginated response
            PaginatedResponse<CartItemResponse> cacheResponse = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages);
            //set cache
            await cacheServices.SetAsync(key, cacheResponse, cancellationToken);
            return Result.Success(cacheResponse, $"fetch cart items of user {currentUser.User!.Username} successfully");
        }
    }
}