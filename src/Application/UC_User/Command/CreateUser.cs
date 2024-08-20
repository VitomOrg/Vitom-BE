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
    ) : IRequest<Result>;

    public class CreateUserHandler(IVitomDbContext context) : IRequestHandler<CreateUserCommand, Result>
    {
        public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            User? checkingUser = await context.Users.SingleOrDefaultAsync(u => u.Id.Equals(request.Id), cancellationToken);
            if (checkingUser is not null) return Result.Error("User is already exist !");
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
            return Result.SuccessWithMessage("User is created successfully !");
        }
    }

}