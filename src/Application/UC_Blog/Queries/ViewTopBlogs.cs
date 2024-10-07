using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Queries;

public class ViewTopBlogs
{
    public record Query : IRequest<Result<Response>>;

    public record Response(
        IEnumerable<BlogDetailResponse> TopBlogs
    );

    public record BlogDetailResponse(
        Guid Id,
        string Title,
        DateTimeOffset CreatedAt,
        string Author
    )
    {

        public static BlogDetailResponse FromEntity(Blog blog)
        => new(
            Id: blog.Id,
            Title: blog.Title,
            CreatedAt: blog.CreatedAt,
            Author: blog.User.Username
        );
    };

    public class Handler(IVitomDbContext context) : IRequestHandler<Query, Result<Response>>
    {
        public async Task<Result<Response>> Handle(Query request, CancellationToken cancellationToken)
        {
            int daysInMonth = DateTime.DaysInMonth(DateTimeOffset.UtcNow.Year, DateTimeOffset.UtcNow.Month);
            IEnumerable<BlogDetailResponse> query = await context
                .Blogs
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(b => b.DeletedAt == null)
                .Include(b => b.User)
                .OrderByDescending(b => b.TotalVisit / daysInMonth)
                .Select(b => BlogDetailResponse.FromEntity(b))
                .Take(5)
                .ToListAsync(cancellationToken);
            return Result.Success(new Response(query));
        }
    }
}