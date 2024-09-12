using Application.Contracts;
using Application.Responses.CollectionResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Collection.Commands;

public class LikedCollection
{
    public record Command(Guid CollectionId, string UserId)
        : IRequest<Result<LikeCollectionResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result<LikeCollectionResponse>>
    {
        public async Task<Result<LikeCollectionResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            // Get existing like collection
            LikeCollection? likeCollection = await context
                .LikeCollections.AsNoTracking()
                .Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            // Check if user already liked collection
            if (likeCollection is not null && likeCollection.DeletedAt is null)
                return Result.Success(
                    new LikeCollectionResponse(likeCollection.Id, likeCollection.CreatedAt)
                    {
                        Id = likeCollection.Id,
                        CreatedAt = likeCollection.CreatedAt
                    },
                    $"User {request.UserId} already liked collection {request.CollectionId}"
                );

            // Create new like collection
            likeCollection = new() { CollectionId = request.CollectionId, UserId = request.UserId };

            // Add like collection to database
            context.LikeCollections.Add(likeCollection);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                new LikeCollectionResponse(likeCollection.Id, likeCollection.CreatedAt)
                {
                    Id = likeCollection.Id,
                    CreatedAt = likeCollection.CreatedAt
                },
                $"User {request.UserId} already liked collection {request.CollectionId}"
            );
        }
    }
}
