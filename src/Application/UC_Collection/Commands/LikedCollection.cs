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
                .Where(c => c.Id == request.CollectionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (collection is null)
                return Result.NotFound("Collection not found");

            // Get existing like collection
            LikeCollection? likeCollection = await context
                .LikeCollections.AsNoTracking()
                .Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == currentUser.User.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (likeCollection is null)
                return await CreateNewLikeCollection(request, cancellationToken);

            if (likeCollection.DeletedAt is null)
                return Result.NoContent();

            return await RestoreLikeCollection(likeCollection, cancellationToken);
        }

        private async Task<Result<LikeCollectionResponse>> CreateNewLikeCollection(
            Command request,
            CancellationToken cancellationToken
        )
        {
            var newLikeCollection = new LikeCollection
            {
                CollectionId = request.CollectionId,
                UserId = currentUser.User.Id
            };

            context.LikeCollections.Add(newLikeCollection);
            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(newLikeCollection);
        }

        private async Task<Result<LikeCollectionResponse>> RestoreLikeCollection(
            LikeCollection likeCollection,
            CancellationToken cancellationToken
        )
        {
            likeCollection.DeletedAt = null;
            context.LikeCollections.Update(likeCollection);
            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(likeCollection);
        }

        private static Result<LikeCollectionResponse> CreateSuccessResult(
            LikeCollection likeCollection
        )
        {
            var response = new LikeCollectionResponse(likeCollection.Id, likeCollection.CreatedAt);

            return Result.Success(
                response,
                $"User {likeCollection.UserId} already liked collection {likeCollection.CollectionId}"
            );
        }
    }
}
