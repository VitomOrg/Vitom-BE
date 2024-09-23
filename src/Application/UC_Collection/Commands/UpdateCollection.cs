using Application.Contracts;
using Application.Responses.CollectionResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Commands;

public class UpdateCollection
{
    public record Command
    (
        Guid Id,
        string Name = "",
        string Description = "",
        bool IsPublic = false
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if user is not found
            if (currentUser.User is null || currentUser.User.DeletedAt != null) return Result.Forbidden();
            // Get existing collection
            Collection? collection = await context.Collections
                .SingleOrDefaultAsync(c => c.Id == request.Id && c.DeletedAt == null, cancellationToken);
            //check if collection is not found
            if (collection is null) return Result.NotFound();
            //check if user is not the owner of the collection
            if (collection.UserId != currentUser.User!.Id) return Result.Forbidden();
            //update collection
            collection.Update(
                name: request.Name,
                description: request.Description,
                isPublic: request.IsPublic
            );
            //Save changes to the database
            await context.SaveChangesAsync(cancellationToken);
            return Result.NoContent();
        }
    }
}