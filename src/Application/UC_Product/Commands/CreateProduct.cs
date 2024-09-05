using Application.Contracts;
using Application.Mappers.ProductMappers;
using Application.Responses.ProductResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.UC_Product.Commands;

public class CreateProduct
{
    public record Command
    (
        [Required]
        [EnumDataType(typeof(LicenseEnum), ErrorMessage = "Invalid License value.")]
        LicenseEnum License,

        [Required, MinLength(1, ErrorMessage = "Name is required.")]
        string Name,

        string Description,

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to zero.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number with up to 2 decimal places.")]
        decimal Price,

        [Required, Url(ErrorMessage = "Invalid URL format.")]
        string DownloadUrl
    ) : IRequest<Result<CreateProductResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateProductResponse>>
    {
        public async Task<Result<CreateProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // Validate the request
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(request);
            bool isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);

            if (!isValid)
            {
                // Get the first validation error message
                var firstErrorMessage = validationResults.First().ErrorMessage;
                return Result.Error(firstErrorMessage);
            }

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
            // add to db
            context.Products.Add(newProduct);
            // save changes
            await context.SaveChangesAsync(cancellationToken);
            // return result with mapped object
            return Result.Success(newProduct.MapToCreateProductResponse(), $"Create new {request.Name} product successfully");
        }
    }

}
