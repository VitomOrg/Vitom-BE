using Application.Contracts;
using Application.Mappers.SoftwareMappers;
using Application.Responses.SoftwareResponses;
using Ardalis.Result;
using Domain.Entities;
using Domain.Enums;
using Domain.Primitives;
using MediatR;

namespace Application.UC_Software.Commands;

public class CreateSoftware
{
    public record Command(
        string Name,
        string Description
    ) : IRequest<Result<CreateSoftwareResponse>>;
    public class Handler(IVitomDbContext context, CurrentUser currentUser) : IRequestHandler<Command, Result<CreateSoftwareResponse>>
    {
        public async Task<Result<CreateSoftwareResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            // check if user is admin
            if (!IsCurrentUserAdmin(currentUser.User!)) return Result.Forbidden();
            // init new software object
            Software newSoftware = new()
            {
                Name = request.Name,
                Description = request.Description
            };
            // add to db
            context.Softwares.Add(newSoftware);
            // save changes
            await context.SaveChangesAsync(cancellationToken);
            // return result with mapped object
            return Result.Success(newSoftware.MapToCreateSoftwareResponse(), $"Create new {request.Name} software successfully");
        }
        private static bool IsCurrentUserAdmin(User checkingUser)
            => checkingUser.Role.Equals(RolesEnum.Admin);
    }
}