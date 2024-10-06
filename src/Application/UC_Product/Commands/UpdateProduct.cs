using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Product.Commands;

public class UpdateProduct
{
    public record Command(
        Guid Id,
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
    ) : IRequest<Result>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser, IFirebaseService firebaseService) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            //check if user is not organization
            if ((!currentUser.User!.IsOrganization() && !currentUser.User.IsAdmin())
                || currentUser.User.DeletedAt != null) return Result.Forbidden();
            //get updating product
            Product? updatingProduct = await context.Products
            .Include(product => product.ProductSoftwares)
            .Include(product => product.ProductTypes)
            .Include(product => product.ProductImages)
            .Include(product => product.ModelMaterials)
            .Include(product => product.Model)
            .SingleOrDefaultAsync(p => p.Id.Equals(request.Id) && p.DeletedAt == null, cancellationToken);
            if (updatingProduct is null) return Result.NotFound();
            // check if user is owner
            if (!updatingProduct.UserId.Equals(currentUser.User.Id) && !currentUser.User.IsAdmin()) return Result.Forbidden();

            //remove all current relationships
            context.ProductTypes.RemoveRange(updatingProduct.ProductTypes);
            context.ProductSoftwares.RemoveRange(updatingProduct.ProductSoftwares);
            context.ProductImages.RemoveRange(updatingProduct.ProductImages);
            context.ModelMaterials.RemoveRange(updatingProduct.ModelMaterials);
            //remove files in firebase directory
            List<Task<bool>> tasksDelete = [];
            foreach (var productImage in updatingProduct.ProductImages)
            {
                tasksDelete.Add(firebaseService.DeleteFile(productImage.Url));
            }
            foreach (var modelMaterial in updatingProduct.ModelMaterials)
            {
                tasksDelete.Add(firebaseService.DeleteFile(modelMaterial.Url));
            }
            tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Fbx));
            tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Obj));
            tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Glb));
            await Task.WhenAll(tasksDelete);
            if (tasksDelete.Any(t => !t.Result)) return Result.Error("Delete files failed");

            //check product types are existed
            IEnumerable<Type> checkingTypes = context.Types.Where(t => request.TypeIds.Contains(t.Id) && t.DeletedAt == null);
            if (checkingTypes.Count() != request.TypeIds.Length) return Result.NotFound($"Types with id {string.Join("", request.TypeIds)} are not existed");
            // types - add product types
            foreach (Type type in checkingTypes)
            {
                context.ProductTypes.Add(new ProductType { ProductId = updatingProduct.Id, TypeId = type.Id });
            }
            //check product softwares are existed
            IEnumerable<Software> checkingSoftwares = context.Softwares.Where(s => request.SoftwareIds.Contains(s.Id) && s.DeletedAt == null);
            if (checkingSoftwares.Count() != request.SoftwareIds.Length) return Result.NotFound($"Softwares with id {string.Join("", request.SoftwareIds)} are not existed");
            // softwares - add product softwares
            foreach (Software software in checkingSoftwares)
            {
                context.ProductSoftwares.Add(new ProductSoftware { ProductId = updatingProduct.Id, SoftwareId = software.Id });
            }
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
                context.ProductImages.Add(new ProductImage { ProductId = updatingProduct.Id, Url = imageUrl });
            }
            // model materials - add product model materials
            List<Task<string>> tasksModelMaterials = [];
            // upload model materials
            foreach (var modelMaterial in request.ModelMaterials)
            {
                tasksModelMaterials.Add(firebaseService.UploadFile(modelMaterial.FileName, modelMaterial, "model-materials"));
            }
            // await all tasks are finished
            string[] modelMaterialUrls = await Task.WhenAll(tasksModelMaterials);
            foreach (var modelMaterialUrl in modelMaterialUrls)
            {
                context.ModelMaterials.Add(new ModelMaterial { ProductId = updatingProduct.Id, Url = modelMaterialUrl });
            }
            // Add model files
            List<Task<string>> modelTasks = [];
            modelTasks.Add(firebaseService.UploadFile(request.Fbx.FileName, request.Fbx, "models"));
            modelTasks.Add(firebaseService.UploadFile(request.Obj.FileName, request.Obj, "models"));
            modelTasks.Add(firebaseService.UploadFile(request.Glb.FileName, request.Glb, "models"));
            string[] modelUrls = await Task.WhenAll(modelTasks);
            updatingProduct.Model.Update(
                modelUrls[0],
                modelUrls[1],
                modelUrls[2]
            );
            //update product
            updatingProduct.Update(
                license: request.License,
                name: request.Name,
                description: request.Description,
                price: request.Price
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
