using Application.Caches.Events;
using Application.Contracts;
using Application.Mappers.TypeMappers;
using Application.Responses.TypeResponses;
using Ardalis.Result;
using Domain.Primitives;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Type = Domain.Entities.Type;

namespace Application.UC_Type.Commands;

public class CreateType
{
    public record Command(string Name, string Description) : IRequest<Result<CreateTypeResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser)
        : IRequestHandler<Command, Result<CreateTypeResponse>>
    {
        public async Task<Result<CreateTypeResponse>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            // check if user is admin
            if (!currentUser.User!.IsAdmin())
                return Result.Forbidden();

            if (context.Types.Any(t => EF.Functions.Like(t.Name, $"{request.Name}")))
                return Result.Error("Type name already exists");

            Type newType = new() { Name = request.Name, Description = request.Description };

            context.Types.Add(newType);
            newType.AddDomainEvent(new EntityCreated.Event("type"));
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                newType.MapToCreateTypeResponse(),
                $"Create new {request.Name} type successfully"
            );
        }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Please enter a valid name maxiumum 100 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required")
                .MaximumLength(250).WithMessage("Please enter a valid name maxiumum 250 characters");
        }
    }
}