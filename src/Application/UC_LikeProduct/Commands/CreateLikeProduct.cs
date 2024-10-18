using Application.Contracts;
using Application.Mappers.LikeProductMappers;
using Application.Responses.LikeProductResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_LikeProduct.Commands;

public class CreateLikeProduct
{
    public record Command(
        Guid ProductId
    ) : IRequest<Result<CreateLikeProductResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateLikeProductResponse>>
    {
        public async Task<Result<CreateLikeProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if product does not exist
            Product? product = await context
                .Products.AsNoTracking()
                .Where(p => p.DeletedAt == null)
                .Where(p => p.Id == request.ProductId)
                .FirstOrDefaultAsync(cancellationToken);
            if (product is null) return Result.NotFound("Product not found");
            //get existing like product
            LikeProduct? likeProduct = await context
                .LikeProducts.AsNoTracking()
                .Where(p => p.ProductId == request.ProductId)
                .Where(p => p.UserId == currentUser.User!.Id)
                .FirstOrDefaultAsync(cancellationToken);
            return likeProduct switch
            {
                null
                    => await CreateNewLikeProduct(
                        product,
                        request.ProductId,
                        cancellationToken
                    ),
                { DeletedAt: null }
                    => await DislikeProduct(likeProduct, product, cancellationToken),
                _ => await RestoreLikeProduct(likeProduct, product, cancellationToken)
            };
        }

        public async Task<CreateLikeProductResponse> CreateNewLikeProduct(
            Product product,
            Guid productId,
            CancellationToken cancellationToken
        )
        {
            //add new like product
            LikeProduct likeProduct = new()
            {
                UserId = currentUser.User!.Id,
                ProductId = productId
            };
            context.LikeProducts.Add(likeProduct);
            //update total liked count
            product.TotalLiked++;
            context.Products.Update(product);
            //Save changes to db
            await context.SaveChangesAsync(cancellationToken);
            return CreateSuccessResult(likeProduct, "liked");
        }

        public async Task<Result<CreateLikeProductResponse>> DislikeProduct(
            LikeProduct likeProduct,
            Product product,
            CancellationToken cancellationToken
        )
        {
            likeProduct.Delete();
            product.TotalLiked--;
            context.LikeProducts.Update(likeProduct);
            context.Products.Update(product);

            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(likeProduct, "disliked");
        }

        private async Task<Result<CreateLikeProductResponse>> RestoreLikeProduct(
            LikeProduct likeProduct,
            Product product,
            CancellationToken cancellationToken
        )
        {
            product.TotalLiked++;
            likeProduct.DeletedAt = null;
            context.Products.Update(product);
            context.LikeProducts.Update(likeProduct);

            await context.SaveChangesAsync(cancellationToken);

            return CreateSuccessResult(likeProduct, "liked");
        }

        private static Result<CreateLikeProductResponse> CreateSuccessResult(
            LikeProduct likeProduct,
            string action
        )
        {

            return Result.Success(
                likeProduct.MapToCreateLikeProductResponse(),
                $"User {likeProduct.UserId} {action} product {likeProduct.Id}"
            );
        }
    }

}