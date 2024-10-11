using Ardalis.Result;
using Domain.Primitives;
using MediatR;

namespace Application.UC_User.Queries;

public class GetUserRole
{
    public record Command() : IRequest<Result<string>>;

    public class Handler(CurrentUser currentUser) : IRequestHandler<Command, Result<string>>
    {
        public Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Result.Success(currentUser.User!.Role.ToString()));
        }
    }
}