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

    public class Handler(IVitomDbContext context, ICacheServices services, CurrentUser currentUser) : IRequestHandler<Query, Result<PaginatedResponse<CartItemResponse>>>
    {
        public async Task<Result<PaginatedResponse<CartItemResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            //check if user is null
            if (currentUser.User is null) return Result.Forbidden();
            //set key
            string key = $"cart:{currentUser.User!.Id}-sortascbycreateat{request.AscByCreatedAt}";
            //get data from cache
            PaginatedResponse<CartItemResponse>? cacheResponse = await services.GetAsync<PaginatedResponse<CartItemResponse>>(key, cancellationToken);
            if (cacheResponse is not null) return Result.Success(cacheResponse, $"fetch cart items of user {currentUser.User!.Username} successfully");
            //get data from db
            IQueryable<CartItem> items = context.CartItems
                .AsNoTracking()
                .Include(ci => ci.Product)
                .Include(ci => ci.Cart)
                .Where(ci => ci.Cart.UserId == currentUser.User!.Id);
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
            cacheResponse = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );
            //set cache
            await services.SetAsync(key, cacheResponse, cancellationToken);
            return Result.Success(cacheResponse, $"fetch cart items of user {currentUser.User!.Username} successfully");
        }
    }
}