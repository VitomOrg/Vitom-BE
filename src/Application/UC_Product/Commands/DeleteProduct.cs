using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Product.Commands;

public class DeleteProduct
{
    public record Command(
        Guid Id
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if current user is organization
            if (!currentUser.User!.IsOrganization()) return Result.Forbidden();
            //get deleting product
            Product? deletingProduct = await context.Products.SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (deletingProduct is null) return Result.NotFound();
            //if deleted at is not null means already deleted
            if (deletingProduct.DeletedAt is not null) return Result.Error($"Product with id {request.Id} has already been deleted");
            //soft delete product
            deletingProduct.Delete();
            await context.SaveChangesAsync(cancellationToken);
            //return result
            return Result.NoContent();
        }
    }

}
