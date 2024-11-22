using Application.Contracts;
using Ardalis.Result;
using Domain.Enums;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.UC_File;

public class UploadFile
{
    public record Command(FileEnum FileEnum, IFormFile File) : IRequest<Result<string>>;

    public class Handler(IFirebaseService firebaseService, CurrentUser currentUser) : IRequestHandler<Command, Result<string>>
    {
        public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.File.OpenReadStream().CanSeek)
            {
                request.File.OpenReadStream().Position = 0;
            }
            if (FileEnum.Model.Equals(request.FileEnum))
            {
                if (!request.File.FileName.ToLower().EndsWith(".fbx")
                    && !request.File.FileName.ToLower().EndsWith(".obj")
                    && !request.File.FileName.ToLower().EndsWith(".glb"))
                {
                    return Result.Error("Model files must be end with '.fbx' , '.obj' or '.glb'");
                }
            }
            return await firebaseService.UploadFile(
                request.File.FileName,
                request.File,
                request.FileEnum.ToString().ToLower() switch
                {
                    "blog" => "blogs",
                    "product" => "products",
                    "model" => "models",
                    "modelmaterial" => "model-materials",
                    _ => "products"
                });
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.File)
                .Must(HaveValidFile).WithMessage("file size must be less than 10MB");
        }

        private bool HaveValidFile(IFormFile file)
            => file.Length < 10240000;
    }
}