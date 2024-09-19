using Application.Contracts;
using Application.Mappers.BlogMappers;
using Application.Responses.BlogResponses;
using Application.Responses.Shared;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Queries;

public class GetBlogs
{
    public record Query(
        int PageSize,
        int PageIndex,
        string Keyword
    ) : IRequest<Result<PaginatedResponse<BlogDetailResponse>>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<PaginatedResponse<BlogDetailResponse>>>
    {
        public async Task<Result<PaginatedResponse<BlogDetailResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            // set key
            string key = $"blogs-keyword{request.Keyword}-pagesize{request.PageSize}-pageindex-{request.PageIndex}";

            // get cache result
            PaginatedResponse<BlogDetailResponse>? cacheResult = await cacheServices.GetAsync<PaginatedResponse<BlogDetailResponse>>(key, cancellationToken);
            if (cacheResult is not null) return Result.Success(cacheResult, "Get blogs successfully !");

            // get query
            IQueryable<Blog> query = context
                .Blogs
                .AsNoTracking()
                .Include(b => b.User)
                .Include(b => b.Images)
                .Where(b => (b.Title.ToLower().Contains(request.Keyword.ToLower()) || b.Content.ToLower().Contains(request.Keyword.ToLower())) && b.DeletedAt == null);

            // get total pages
            int totalPages = (int)Math.Ceiling((decimal)query.Count() / request.PageSize);

            // get result
            IEnumerable<BlogDetailResponse> result = await query
                .Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(b => b.MapToBlogDetailResponse())
                .ToListAsync(cancellationToken);

            // map to paginated result
            cacheResult = new(
                Data: result,
                PageIndex: request.PageIndex,
                PageSize: request.PageSize,
                TotalPages: totalPages
            );

            // set cache
            await cacheServices.SetAsync(key, cacheResult, cancellationToken);

            return Result.Success(cacheResult, "Get blogs successfully !");
        }
    }
}