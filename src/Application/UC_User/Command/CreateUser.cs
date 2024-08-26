using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Command;

public class CreateUser
{
    public record CreateUserCommand(
        string Id,
        string Username,
        string PhoneNumber,
        string Email
    ) : IRequest<Result<User>>;

    public class CreateUserHandler(IVitomDbContext context) : IRequestHandler<CreateUserCommand, Result<User>>
    {
        public async Task<Result<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User? checkingUser = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
            if (checkingUser is not null) return Result.Success(checkingUser);
            checkingUser = new()
            {
                Id = request.Id,
                Username = request.Username,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Role = Domain.Enums.RolesEnum.Customer
            };
            await context.Users.AddAsync(checkingUser, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success(checkingUser);
        }
    }

}