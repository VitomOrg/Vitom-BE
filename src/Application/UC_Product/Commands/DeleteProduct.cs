using Application.Caches.Events;
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
            if (!currentUser.User!.IsOrganization() || currentUser.User.DeletedAt != null) return Result.Forbidden();
            //get deleting product
            Product? deletingProduct = await context.Products
            .Include(product => product.ProductSoftwares)
            .Include(product => product.ProductTypes)
            .Include(product => product.ProductImages)
            .Include(product => product.ModelMaterials)
            .Include(product => product.Model)
            .Where(product => product.DeletedAt == null)
            .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);

            if (deletingProduct is null) return Result.NotFound();
            //if deleted at is not null means already deleted
            if (deletingProduct.DeletedAt is not null) return Result.Error($"Product with id {request.Id} has already been deleted");
            //check if user is owner
            if (!deletingProduct.UserId.Equals(currentUser.User.Id)) return Result.Forbidden();
            //soft delete for product types
            foreach (ProductType productType in deletingProduct.ProductTypes)
            {
                productType.Delete();
            }
            //soft delete for product softwares
            foreach (ProductSoftware productSoftware in deletingProduct.ProductSoftwares)
            {
                productSoftware.Delete();
            }
            //soft delete for product images
            foreach (ProductImage productImage in deletingProduct.ProductImages)
            {
                productImage.Delete();
            }
            //soft delete for model materials
            foreach (ModelMaterial modelMaterial in deletingProduct.ModelMaterials)
            {
                modelMaterial.Delete();
            }
            //soft delete for model
            deletingProduct.Model?.Delete();
            //soft delete product
            deletingProduct.Delete();
            deletingProduct.AddDomainEvent(new EntityRemove.Event("product"));
            await context.SaveChangesAsync(cancellationToken);
            //return result
            return Result.NoContent();
        }
    }

}