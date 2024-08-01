using Application.Contracts;
using Ardalis.Result;
using Domain.DomainEvent;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UC_User;

public class GetAllUsers
{
    public record GetAllUsersQuery(int PageNumber, int PageSize) : IRequest<Result<IEnumerable<User>>>;

    public sealed class GetAllUsersHandler(IVitomDbContext context, ICacheServices cacheServices, IPublisher publisher) : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<User>>>
    {
        public async Task<Result<IEnumerable<User>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            string key = "users-all";
            await publisher.Publish(new UserGetsDomainEvent(DateTimeOffset.UtcNow), cancellationToken);
            IEnumerable<User>? usersResponse = await cacheServices.GetAsync<IEnumerable<User>>(key, cancellationToken);
            if (usersResponse is not null)
            {
                return Result.Success(usersResponse);
            }
            IEnumerable<User> users = await context.Users.Skip(request.PageSize * (request.PageNumber - 1))
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);
            await cacheServices.SetAsync(key, users, cancellationToken);
            return Result.Success(users);
        }
    }
}