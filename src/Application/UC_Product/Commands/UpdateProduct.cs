using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_Product.Commands;

public class UpdateProduct
{
    public record Command(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        string DownloadUrl
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if user is not organization
            if (!currentUser.User!.IsOrganization()) return Result.Forbidden();
            //get updating product
            Product? updatingProduct = await context.Products.SingleOrDefaultAsync(p => p.Id.Equals(request.Id) && p.DeletedAt == null, cancellationToken);
            if (updatingProduct is null) return Result.NotFound();
            //update product
            updatingProduct.Update(
                name: request.Name,
                description: request.Description,
                price: request.Price,
                downloadUrl: request.DownloadUrl
            );
            //save to db
            await context.SaveChangesAsync(cancellationToken);
            //return final result
            return Result.NoContent();
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative number")
                .Must(HaveValidDecimalPlaces).WithMessage("Price must have up to two decimal places"); ;
            RuleFor(x => x.DownloadUrl).Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)).WithMessage("DownloadUrl must be a valid URL");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            // Ensure that price has at most two decimal places
            return decimal.Round(price, 2) == price;
        }
    }
}
