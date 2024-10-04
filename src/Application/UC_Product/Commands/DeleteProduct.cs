using Application.Contracts;
using Ardalis.Result;
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
            int result = await context.Products
            .Include(product => product.ProductSoftwares)
            .Include(product => product.ProductTypes)
            .Include(product => product.ProductImages)
            .Include(product => product.ModelMaterials)
            .Where(p => p.Id.Equals(request.Id) && p.DeletedAt == null)
            .ExecuteUpdateAsync(e =>
                e.SetProperty(p => p.ProductTypes.Any(pt => pt.DeletedAt == null), false)
                    .SetProperty(p => p.ProductSoftwares.Any(ps => ps.DeletedAt == null), false)
                    .SetProperty(p => p.ProductImages.Any(pi => pi.DeletedAt == null), false)
                    .SetProperty(p => p.ModelMaterials.Any(mm => mm.DeletedAt == null), false)
                    .SetProperty(p => p.DeletedAt, DateTime.Now)
               , cancellationToken)
            ;
            if (result < 0) return Result.NotFound();
            return Result.NoContent();
        }
    }

}
