using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Commands;

public class DislikedCollection
{
    public record Command(Guid CollectionId, string UserId) : IRequest<Result>;

    public class Handler(IVitomDbContext context) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            // Get existing like collection
            LikeCollection? likeCollection = await context
                .LikeCollections.Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == request.UserId)
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
