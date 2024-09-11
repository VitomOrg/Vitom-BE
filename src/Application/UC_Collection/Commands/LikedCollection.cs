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
    public record Command(Guid CollectionId, string UserId) : IRequest<LikeCollectionResponse>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, LikeCollectionResponse>
    {
        public async Task<LikeCollectionResponse> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            IQueryable<LikeCollection> query = context
                .LikeCollections.AsNoTracking()
                .Where(c => c.CollectionId == request.CollectionId)
                .Where(c => c.UserId == request.UserId);

            LikeCollection? likeCollection = await query.FirstOrDefaultAsync(cancellationToken);

            var returnResult = Result.Success(
                new LikeCollectionResponse(likeCollection.Id, likeCollection.CreatedAt)
                {
                    Id = likeCollection.Id,
                    CreatedAt = likeCollection.CreatedAt
                },
                $"User {request.UserId} already liked collection {request.CollectionId}"
            );

            // Check if user already liked collection
            if (likeCollection is not null && likeCollection.DeletedAt is null)
                return returnResult;

            likeCollection = new() { CollectionId = request.CollectionId, UserId = request.UserId };

            context.LikeCollections.Add(likeCollection);

            await context.SaveChangesAsync(cancellationToken);

            return returnResult;
        }
    }
}
