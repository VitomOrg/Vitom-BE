using Application.Responses.User_Responses;
using Domain.Entities;

namespace Application.Mapper.UserMappers;

public static class GetUserByIdResponseMapper
{
    public static GetUserByIdResponse MapToGetUserByIdResponse(this User user)
        => new GetUserByIdResponse();
}