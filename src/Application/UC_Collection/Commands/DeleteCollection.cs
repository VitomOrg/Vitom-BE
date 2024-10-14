using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Commands;

public class DeleteCollection
{
    public record Command(Guid Id) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is admin
            Collection? gettingColletion = await context
                                                .Collections
                                                .Where(c => c.DeletedAt == null)
                                                .SingleOrDefaultAsync(c => c.Id.Equals(request.Id), cancellationToken);

            if (gettingColletion is null) return Result.NotFound();
            // check if user is the owner of the collection
            if (gettingColletion.UserId != currentUser.User!.Id) return Result.Forbidden();
            // soft delete collection
            gettingColletion.Delete();
            gettingColletion.AddDomainEvent(new EntityRemove.Event("collection"));
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}