using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;

namespace Application.UC_Product.Commands;

public class CreateProduct
{
    public record Command
    (
        LicenseEnum License,
        string Name,
        string Description,
        decimal Price,
        string DownloadUrl,
        Guid[] TypeIds,
        Guid[] SoftwareIds,
        IFormFileCollection Images
    ) : IRequest<Result<CreateProductResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result<CreateProductResponse>>
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

            //check product types are existed
            foreach (var typeIntput in request.TypeIds)
            {
                if (!context.Types.Any(t => t.Id == typeIntput && t.DeletedAt == null))
                {
                    return Result.NotFound($"Type with id {typeIntput} is not existed");
                }
            }
            // add product types
            foreach (var typeId in request.TypeIds)
            {
                newProduct.ProductTypes.Add(new ProductType { ProductId = newProduct.Id, TypeId = typeId });
            }

            //check product softwares are existed
            foreach (var softwareId in request.SoftwareIds)
            {
                if (!context.Softwares.Any(s => s.Id == softwareId && s.DeletedAt == null))
                {
                    return Result.NotFound($"Software with id {softwareId} is not existed");
                }
            }
            // add product softwares
            foreach (var softwareId in request.SoftwareIds)
            {
                newProduct.ProductSoftwares.Add(new ProductSoftware { ProductId = newProduct.Id, SoftwareId = softwareId });
            }
            // add product images
            foreach (var image in request.Images)
            {
                string imageUrl = await firebaseService.UploadFile(image.Name, image, "products");
                newProduct.ProductImages.Add(new ProductImage { ProductId = newProduct.Id, Url = imageUrl });
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
            RuleFor(x => x.Images)
                .Must(HaveValidFiles)
                .WithMessage("Each ImageUrl must be a valid URL");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            // Ensure that price has at most two decimal places
            return decimal.Round(price, 2) == price;
        }

        private bool HaveValidFiles(IFormFileCollection images)
            => images.All(image => image.Length < 10240000);

        private bool IsValidHexColorCode(string code)
        {
            Regex HexColorRegex = new Regex(@"^#[0-9A-Fa-f]{6}$", RegexOptions.Compiled);
            return !string.IsNullOrWhiteSpace(code) && HexColorRegex.IsMatch(code);
        }
    }

}
