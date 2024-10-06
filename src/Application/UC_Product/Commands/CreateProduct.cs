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
using Type = Domain.Entities.Type;

namespace Application.UC_Product.Commands;

public class CreateProduct
{
    public record Command
    (
        LicenseEnum License,
        string Name,
        string Description,
        decimal Price,
        Guid[] TypeIds,
        Guid[] SoftwareIds,
        List<IFormFile> Images,
        List<IFormFile> ModelMaterials,
        IFormFile Fbx,
        IFormFile Obj,
        IFormFile Glb
    ) : IRequest<Result<CreateProductResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result<CreateProductResponse>>
    {
        public async Task<Result<CreateProductResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is Organization
            if (!currentUser.User!.IsOrganization() || currentUser.User.DeletedAt != null) return Result.Forbidden();
            // init new product object
            Product newProduct = new()
            {
                UserId = currentUser.User!.Id,
                License = request.License,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                DownloadUrl = string.Empty
            };
            context.Products.Add(newProduct);
            //check product types are existed
            IEnumerable<Type> checkingTypes = context.Types.Where(t => request.TypeIds.Contains(t.Id) && t.DeletedAt == null);
            if (checkingTypes.Count() != request.TypeIds.Length) return Result.NotFound($"Types with id {string.Join(", ", request.TypeIds)}  are not existed");
            // types - add product types
            newProduct.ProductTypes = checkingTypes.Select(t => new ProductType { ProductId = newProduct.Id, TypeId = t.Id }).ToList();

            //check product softwares are existed
            IEnumerable<Software> checkingSoftwares = context.Softwares.Where(s => request.SoftwareIds.Contains(s.Id) && s.DeletedAt == null);
            if (checkingSoftwares.Count() != request.SoftwareIds.Length) return Result.NotFound($"Softwares with id {string.Join(", ", request.SoftwareIds)} are not existed");
            // software - add product softwares
            newProduct.ProductSoftwares = checkingSoftwares.Select(s => new ProductSoftware { ProductId = newProduct.Id, SoftwareId = s.Id }).ToList();
            // images - add product images
            List<Task<string>> tasks = [];
            // upload images
            foreach (var image in request.Images)
            {
                tasks.Add(firebaseService.UploadFile(image.FileName, image, "products"));
            }
            // await all tasks are finished
            string[] imageUrls = await Task.WhenAll(tasks);
            foreach (var imageUrl in imageUrls)
            {
                newProduct.ProductImages.Add(new ProductImage { ProductId = newProduct.Id, Url = imageUrl });
            }
            // material - add product model materials
            List<Task<string>> modelMaterialTasks = [];
            foreach (var modelMaterial in request.ModelMaterials)
            {
                modelMaterialTasks.Add(firebaseService.UploadFile(modelMaterial.FileName, modelMaterial, "model-materials"));
            }
            string[] modelMaterialUrls = await Task.WhenAll(modelMaterialTasks);
            foreach (var materialUrl in modelMaterialUrls)
            {
                newProduct.ModelMaterials.Add(new ModelMaterial { ProductId = newProduct.Id, Url = materialUrl });
            }
            // Add model files
            List<Task<string>> modelTasks = [];
            modelTasks.Add(firebaseService.UploadFile(request.Fbx.FileName, request.Fbx, "models"));
            modelTasks.Add(firebaseService.UploadFile(request.Obj.FileName, request.Obj, "models"));
            modelTasks.Add(firebaseService.UploadFile(request.Glb.FileName, request.Glb, "models"));
            string[] modelUrls = await Task.WhenAll(modelTasks);
            newProduct.Model = new Model
            {
                ProductId = newProduct.Id,
                Fbx = modelUrls[0],
                Obj = modelUrls[1],
                Glb = modelUrls[2]
            };
            // zip all images, materials and upload zip file to firebase service
            List<IFormFile> zipFiles = request.Images;
            zipFiles.AddRange(request.ModelMaterials);
            zipFiles.AddRange([request.Fbx, request.Obj, request.Glb]);

            string zipUrl = await firebaseService.UploadFiles(zipFiles, "download-zip-product");
            //update download url for product
            newProduct.DownloadUrl = zipUrl;
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
                .Must(HaveValidDecimalPlaces).WithMessage("Price must have up to two decimal places");
            RuleFor(x => x.Images)
                .Must(HaveValidFiles)
                .WithMessage("Each ImageUrl must be a valid URL");
            RuleFor(x => x.Fbx)
                .Must(HaveValidFile).WithMessage("Fbx file must be a valid file")
                .Must(BeFbxFile).WithMessage("Fbx file must be a Fbx file");
            RuleFor(x => x.Obj)
                .Must(HaveValidFile).WithMessage("Obj file must be a valid file")
                .Must(BeObjFile).WithMessage("Obj file must be a obj file");
            RuleFor(x => x.Glb)
                .Must(HaveValidFile).WithMessage("Glb file must be a valid file")
                .Must(BeGlbFile).WithMessage("Glb file must be a glb file");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            // Ensure that price has at most two decimal places
            return decimal.Round(price, 2) == price;
        }

        private bool HaveValidFiles(List<IFormFile> images)
            => images.All(image => image.Length < 10240000);

        private bool HaveValidFile(IFormFile file)
            => file.Length < 10240000;

        private bool BeGlbFile(IFormFile formFile)
            => formFile.FileName.ToLower().EndsWith(".glb");

        private bool BeObjFile(IFormFile formFile)
            => formFile.FileName.ToLower().EndsWith(".obj");

        private bool BeFbxFile(IFormFile formFile)
            => formFile.FileName.ToLower().EndsWith(".fbx");
    }

}
