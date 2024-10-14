using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Blog.Commands;

public class DeleteBlog
{
    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // get blog
            Blog? deletingBlog =
                await context.Blogs
                    .Where(b => b.DeletedAt == null)
                    .SingleOrDefaultAsync(b => b.Id.Equals(request.Id), cancellationToken);
            if (deletingBlog is null) return Result.NotFound();
            // check if user is owner
            if (!deletingBlog.UserId.Equals(currentUser.User!.Id)) return Result.Forbidden();
            // delete blog
            deletingBlog.Delete();
            deletingBlog.AddDomainEvent(new EntityRemove.Event("blog"));
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}