using Application.Caches.Events;
using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Product.Commands;

public class UpdateProduct
{
    public record Command(
        Guid Id,
        string Name,
        string Description,
        decimal Price,
        Guid[] TypeIds,
        Guid[] SoftwareIds,
        string[] Images,
        string[] ModelMaterials,
        string Fbx,
        string Obj,
        string Glb
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
            .Where(p => p.DeletedAt == null)
            .SingleOrDefaultAsync(p => p.Id.Equals(request.Id), cancellationToken);
            if (updatingProduct is null) return Result.NotFound();
            // check if user is owner
            if (!updatingProduct.UserId.Equals(currentUser.User.Id) && !currentUser.User.IsAdmin()) return Result.Forbidden();

            //remove all current relationships
            context.ProductTypes.RemoveRange(updatingProduct.ProductTypes);
            context.ProductSoftwares.RemoveRange(updatingProduct.ProductSoftwares);
            context.ProductImages.RemoveRange(updatingProduct.ProductImages);
            context.ModelMaterials.RemoveRange(updatingProduct.ModelMaterials);
            //remove files in firebase directory
            IEnumerable<ProductImage> unusedImages = [.. context.ProductImages
                .AsNoTracking()
                .Where(pi => pi.ProductId.Equals(updatingProduct.Id))
                .Where(pi => !request.Images.Contains(pi.Url))];
            List<Task<bool>> tasksDelete = [];
            foreach (var productImage in unusedImages)
            {
                tasksDelete.Add(firebaseService.DeleteFile(productImage.Url));
            }
            IEnumerable<ModelMaterial> unusedModelMaterials = [.. context.ModelMaterials
                .AsNoTracking()
                .Where(mm => mm.ProductId.Equals(updatingProduct.Id))
                .Where(mm => !request.ModelMaterials.Contains(mm.Url))];
            foreach (var modelMaterial in unusedModelMaterials)
            {
                tasksDelete.Add(firebaseService.DeleteFile(modelMaterial.Url));
            }
            if (request.Fbx != updatingProduct.Model.Fbx)
                tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Fbx));
            if (request.Obj != updatingProduct.Model.Obj)
                tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Obj));
            if (request.Glb != updatingProduct.Model.Glb)
                tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.Model.Glb));
            tasksDelete.Add(firebaseService.DeleteFile(updatingProduct.DownloadUrl));
            await Task.WhenAll(tasksDelete);
            if (tasksDelete.Any(t => !t.Result)) return Result.Error("Delete model files failed");

            //check product types are existed
            IEnumerable<Type> checkingTypes = context.Types
                .AsNoTracking()
                .Where(t => t.DeletedAt == null)
                .Where(t => request.TypeIds.Contains(t.Id));
            if (checkingTypes.Count() != request.TypeIds.Length) return Result.NotFound("Some types are not found");
            // types - add product types
            foreach (Type type in checkingTypes)
            {
                context.ProductTypes.Add(new ProductType { ProductId = updatingProduct.Id, TypeId = type.Id });
            }
            //check product softwares are existed
            IEnumerable<Software> checkingSoftwares = context.Softwares
                .AsNoTracking()
                .Where(s => s.DeletedAt == null)
                .Where(s => request.SoftwareIds.Contains(s.Id));
            if (checkingSoftwares.Count() != request.SoftwareIds.Length) return Result.NotFound("Some softwares are not found");
            // softwares - add product softwares
            foreach (Software software in checkingSoftwares)
            {
                context.ProductSoftwares.Add(new ProductSoftware { ProductId = updatingProduct.Id, SoftwareId = software.Id });
            }
            // images - add product images
            context.ProductImages.AddRange(
                request.Images.Select(
                    imageUrl => new ProductImage
                    {
                        ProductId = updatingProduct.Id,
                        Url = imageUrl
                    }));
            // model materials - add product model materials
            context.ModelMaterials.AddRange(
                request.ModelMaterials.Select(
                    modelMaterialUrl => new ModelMaterial
                    {
                        ProductId = updatingProduct.Id,
                        Url = modelMaterialUrl
                    }));
            // Add model files
            updatingProduct.Model.Update(
                request.Fbx,
                request.Obj,
                request.Glb
            );
            //update product
            updatingProduct.Update(
                license: request.Price != 0 ? Domain.Enums.LicenseEnum.Pro : Domain.Enums.LicenseEnum.Free,
                name: request.Name,
                description: request.Description,
                price: request.Price
            );
            // await for zip uploads
            List<string> zipFiles =
            [
                .. request.Images,
                .. request.ModelMaterials,
                .. new[] { request.Fbx, request.Obj, request.Glb },
            ];
            string zipUrl = await firebaseService.UploadFiles(zipFiles, "download-zip-product");
            updatingProduct.DownloadUrl = zipUrl;
            updatingProduct.AddDomainEvent(new EntityUpdated.Event("product"));
            //save to db
            await context.SaveChangesAsync(cancellationToken);
            //return final result
            return Result.SuccessWithMessage("Update product successfully");
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative number")
                .Must(HaveValidDecimalPlaces).WithMessage("Price must have up to two decimal places");
            RuleFor(x => x.Fbx)
                .Must(BeFbxFile).WithMessage("Fbx file must be a Fbx file");
            RuleFor(x => x.Obj)
                .Must(BeObjFile).WithMessage("Obj file must be a obj file");
            RuleFor(x => x.Glb)
                .Must(BeGlbFile).WithMessage("Glb file must be a glb file");
        }

        private bool HaveValidDecimalPlaces(decimal price)
        {
            // Ensure that price has at most two decimal places
            return decimal.Round(price, 2) == price;
        }

        private bool BeGlbFile(string fileName)
            => fileName.ToLower().EndsWith(".glb");

        private bool BeObjFile(string fileName)
            => fileName.ToLower().EndsWith(".obj");

        private bool BeFbxFile(string fileName)
            => fileName.ToLower().EndsWith(".fbx");
    }
}