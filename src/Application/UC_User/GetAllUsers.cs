using Application.Contracts;
using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User;

public class GetAllUsers
{
    public record GetAllUsersQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<User>>>;

    public sealed class GetAllUsersHandler(IVitomDbContext context) : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<User>>>
    {
        public async Task<Result<IEnumerable<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await context.Users.Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            return Result.Success(users);
        }
    }
}