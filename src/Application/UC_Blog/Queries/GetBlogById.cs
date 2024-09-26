using Application.Contracts;
using Application.Mappers.BlogMappers;
using Application.Responses.BlogResponses;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Queries;

public class GetBlogById
{
    public record Query(Guid Id) : IRequest<Result<BlogDetailResponse>>;

    public class Handler(IVitomDbContext context, ICacheServices cacheServices) : IRequestHandler<Query, Result<BlogDetailResponse>>
    {
        public async Task<Result<BlogDetailResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            string key = $"Blog_{request.Id}";
            BlogDetailResponse? cacheBlog = await cacheServices.GetAsync<BlogDetailResponse>(key, cancellationToken);
            if (cacheBlog is not null) return Result.Success(cacheBlog);
            Blog? queryBlog = await context
                                    .Blogs
                                    .AsNoTracking()
                                    .Include(x => x.User)
                                    .Include(x => x.Images)
                                    .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (queryBlog is null) return Result.NotFound("Blog not found");
            // set data to cache
            cacheBlog = queryBlog.MapToBlogDetailResponse();
            await cacheServices.SetAsync(key, cacheBlog, cancellationToken);
            return Result.Success(cacheBlog);
        }
    }
}