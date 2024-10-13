using Application.Contracts;
using Application.Mappers.BlogMappers;
using Application.Responses.BlogResponses;
using Application.UC_Blog.Events;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Queries;

public class GetBlogById
{
    public record Query(Guid Id) : IRequest<Result<BlogDetailResponse>>;

    public class Handler(IVitomDbContext context, IMediator mediator) : IRequestHandler<Query, Result<BlogDetailResponse>>
    {
        public async Task<Result<BlogDetailResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            Blog? queryBlog = await context
                                    .Blogs
                                    .AsNoTracking().IgnoreQueryFilters()
                                    .Include(x => x.User)
                                    .Include(x => x.Images)
                                    .Where(x => x.DeletedAt == null)
                                    .FirstOrDefaultAsync(x => x.Id.Equals(request.Id), cancellationToken);
            if (queryBlog is null) return Result.NotFound("Blog not found");
            await mediator.Publish(new BlogViewed.Event(queryBlog.Id), cancellationToken);
            return Result.Success(queryBlog.MapToBlogDetailResponse());
        }
    }
}