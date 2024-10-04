using Application.Contracts;
using Application.Mappers.CollectionMappers;
using Application.Responses.CollectionResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using FluentValidation;
using MediatR;

namespace Application.UC_Collection.Commands;

public class CreateCollection
{
    public record Command(
        string Name,
        string Description,
        bool IsPublic
    ) : IRequest<Result<CreateCollectionResponse>>;

    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateCollectionResponse>>
    {
        public async Task<Result<CreateCollectionResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            Collection addingCollection = new()
            {
                Name = request.Name,
                Description = request.Description,
                IsPublic = request.IsPublic,
                UserId = currentUser.User!.Id
            };

            context.Collections.Add(addingCollection);
            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(addingCollection.MapToCreateCollectionResponse());
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