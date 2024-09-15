using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.UC_Product.Commands;

public class CreateProduct
{
    public record CreateCustomColorCommand(string Name, string Code);

    public record Command
    (
        LicenseEnum License,
        string Name,
        string Description,
        decimal Price,
        string DownloadUrl,
        IEnumerable<Guid> TypeIds,
        IEnumerable<Guid> SoftwareIds,
        IEnumerable<string> ImageUrls,
        IEnumerable<CreateCustomColorCommand> CustomColors
    ) : IRequest<Result<CreateProductResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateProductResponse>>
    {
        public async Task<Result<CreateProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is Organization
            //if (!currentUser.User!.IsOrganization()) return Result.Forbidden();
            // init new product object
            Product newProduct = new()
            {
                UserId = currentUser.User!.Id,
                License = request.License,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DownloadUrl = request.DownloadUrl
            };
            context.Products.Add(newProduct);
            // add product types
            foreach (var typeId in request.TypeIds)
            {
                newProduct.ProductTypes.Add(new ProductType { ProductId = newProduct.Id, TypeId = typeId });
            }
            // add product softwares
            foreach (var softwareId in request.SoftwareIds)
            {
                newProduct.ProductSoftwares.Add(new ProductSoftware { ProductId = newProduct.Id, SoftwareId = softwareId });
            }
            // add product images
            foreach (var imageUrl in request.ImageUrls)
            {
                newProduct.ProductImages.Add(new ProductImage { ProductId = newProduct.Id, Url = imageUrl });
            }
            // add custom colors
            foreach (var customColor in request.CustomColors)
            {
                newProduct.CustomColors.Add(new CustomColor { ProductId = newProduct.Id, Name = customColor.Name, Code = customColor.Code });
            }
            // save changes
            await context.SaveChangesAsync(cancellationToken);
            // return result with mapped object
            return Result.Success(newProduct.MapToCreateProductResponse(), $"Create new {request.Name} product successfully");
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.License).IsInEnum().WithMessage("License must be 0, 1, Free, or Pro");
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
