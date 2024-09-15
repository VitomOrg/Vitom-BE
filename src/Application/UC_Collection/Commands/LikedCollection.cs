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
    public record Command(Guid CollectionId) : IRequest<Result<LikeCollectionResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result<LikeCollectionResponse>>
    {
        public async Task<Result<LikeCollectionResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            // Check if collection exists
            Collection? collection = await context
                .Collections.AsNoTracking()
                .Where(c => c.Id == request.CollectionId && c.DeletedAt == null)
                .FirstOrDefaultAsync(cancellationToken);

            if (collection is null)
                return Result.NotFound("Collection not found");

            // Get existing like collection
            LikeCollection? likeCollection = await context
                .LikeCollections.AsNoTracking()
                .Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == currentUser.User.Id)
                .FirstOrDefaultAsync(cancellationToken);

            return likeCollection switch
            {
                null
                    => await CreateNewLikeCollection(
                        request.CollectionId,
                        collection,
                        cancellationToken
                    ),
                { DeletedAt: null }
                    => await DislikeCollection(likeCollection, collection, cancellationToken),
                _ => await RestoreLikeCollection(likeCollection, collection, cancellationToken)
            };
        }

        private async Task<Result<LikeCollectionResponse>> CreateNewLikeCollection(
            Guid collectionId,
            Collection collection,
            CancellationToken cancellationToken
        )
        {
            var newLikeCollection = new LikeCollection
            {
                CollectionId = collectionId,
                UserId = currentUser.User.Id
            };

            collection.TotalLiked++;
            context.Collections.Update(collection);
            context.LikeCollections.Add(newLikeCollection);

            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(newLikeCollection, "liked");
        }

        private async Task<Result<LikeCollectionResponse>> DislikeCollection(
            LikeCollection likeCollection,
            Collection collection,
            CancellationToken cancellationToken
        )
        {
            likeCollection.Delete();
            collection.TotalLiked--;
            context.LikeCollections.Update(likeCollection);
            context.Collections.Update(collection);

            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(likeCollection, "disliked");
        }

        private async Task<Result<LikeCollectionResponse>> RestoreLikeCollection(
            LikeCollection likeCollection,
            Collection collection,
            CancellationToken cancellationToken
        )
        {
            collection.TotalLiked++;
            likeCollection.DeletedAt = null;
            context.Collections.Update(collection);
            context.LikeCollections.Update(likeCollection);

            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(likeCollection, "liked");
        }

        private static Result<LikeCollectionResponse> CreateSuccessResult(
            LikeCollection likeCollection,
            string action
        )
        {
            var response = new LikeCollectionResponse(
                likeCollection.Id,
                likeCollection.CreatedAt,
                likeCollection.DeletedAt
            );

            return Result.Success(
                response,
                $"User {likeCollection.UserId} {action} collection {likeCollection.CollectionId}"
            );
        }
    }
}
