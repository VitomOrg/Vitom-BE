using Application.UC_User.Command;
using Ardalis.Result;
using Domain.Entities;
using MediatR;

namespace API.Middlewares;

public class AuthMiddleware(ISender sender) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string issuer = "https://exotic-squid-32.clerk.accounts.dev";
        var claims = context.User.Claims;
        if (claims is null)
        {
            await next.Invoke(context);
            return;
        }
        if (!claims.Any(c => c.Issuer.Equals(issuer, StringComparison.InvariantCultureIgnoreCase)))
        {
            await next.Invoke(context);
            return;
        }
        foreach (var claim in claims)
        {
            Console.WriteLine(claim.Type + "-" + claim.Value);
        }
        string? id = claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        string? username = claims.FirstOrDefault(c => c.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        string? email = claims.FirstOrDefault(c => c.Type.Equals("email", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        string? phoneNumber = claims.FirstOrDefault(c => c.Type.Equals("phonenumber", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        Result result = await sender.Send(new CreateUser.CreateUserCommand(
            Id: null!,
            Username: username,
            PhoneNumber: phoneNumber,
            Email: email
        ));
        if (!result.IsSuccess)
        {
            throw new Exception("Error in adding new user !");
        }
    }
}