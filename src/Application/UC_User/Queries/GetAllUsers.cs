using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User.Queries;

public class GetAllUsers
{
    public class Query : IRequest<Result<List<UserDetailsResponse>>> { }

    public record UserDetailsResponse(
        string Username,
        string Email,
        string Phone,
        string Role
    )
    {
        public static UserDetailsResponse MapToUserDetailsResponse(User user) => new(
            Username: user.Username,
            Email: user.Email,
            Phone: user.PhoneNumber,
            Role: user.Role.ToString()
        );
    };

    public class Handler(IVitomDbContext context) : IRequestHandler<Query, Result<List<UserDetailsResponse>>>
    {
        public async Task<Result<List<UserDetailsResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<UserDetailsResponse> users = await context.Users
                .AsNoTracking()
                .Where(u => u.DeletedAt == null)
                .Select(u => UserDetailsResponse.MapToUserDetailsResponse(u))
                .ToListAsync(cancellationToken);
            return Result.Success(users);
        }
    }
}