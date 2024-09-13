using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Commands;

public class DislikedCollection
{
    public record Command(Guid CollectionId) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // Check if collection exists
            Collection? collection = await context
                .Collections.AsNoTracking()
                .Where(c => c.Id == request.CollectionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (collection is null)
                return Result.NotFound("Collection not found");

            // Get existing like collection
            LikeCollection? likeCollection = await context
                .LikeCollections.Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == currentUser.User.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (
                likeCollection is not null && likeCollection.DeletedAt is not null
                || likeCollection is null
            )
                return Result.NoContent();

            likeCollection.Delete();
            await context.SaveChangesAsync(cancellationToken);

            return Result.NoContent();
        }
    }
}
