using Application.Contracts;
using Application.Mappers.TypeMappers;
using Application.Responses.TypeResponses;
using Ardalis.Result;
using Domain.Primitives;
using MediatR;
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

            Type newType = new() { Name = request.Name, Description = request.Description };

            context.Types.Add(newType);

            await context.SaveChangesAsync(cancellationToken);

            return Result.Success(
                newType.MapToCreateTypeResponse(),
                $"Create new {request.Name} type successfully"
            );
        }
    }
}
