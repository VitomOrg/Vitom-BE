using Ardalis.Result;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Contracts;
using Application.Responses.User_Responses;

namespace Application.UC_User;

public record GetUserByIdQuery(
    Guid Id
) : IRequest<Result<GetUserByIdResponse>>;

public class GetUserByIdHandler(IVitomDbContext context) : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdResponse>>
{
    public async Task<Result<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        await Task.Delay(0);
        return Result.Success();
    }
}