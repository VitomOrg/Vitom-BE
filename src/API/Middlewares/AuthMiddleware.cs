using Application.Contracts;
using Application.UC_User.Command;
using Ardalis.Result;
using Domain.Entities;
using Domain.Primitives;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace API.Middlewares;

public class AuthMiddleware(IVitomDbContext vitomDbContext) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string issuer = "https://exotic-squid-32.clerk.accounts.dev";
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (authHeader.IsNullOrEmpty()
            || !authHeader.Contains("Bearer "))
        {
            await next.Invoke(context);
            return;
        }
        authHeader = authHeader.Replace("Bearer ", "");
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(authHeader);
        var claims = jwtSecurityToken.Claims;
        // var issuercc = claims.FirstOrDefault(c => c.Issuer.Equals(issuer, StringComparison.InvariantCultureIgnoreCase));
        if (!claims.Any(c => c.Issuer.Equals(issuer, StringComparison.InvariantCultureIgnoreCase)))
        {
            await next.Invoke(context);
            return;
        }
        // foreach (var claim in claims)
        // {
        //     Console.WriteLine(claim.Type + "-" + claim.Value);
        // }
        string? id = claims.FirstOrDefault(c => c.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        // Console.WriteLine($"id : {id}");
        string? username = claims.FirstOrDefault(c => c.Type.Equals("username", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        string? email = claims.FirstOrDefault(c => c.Type.Equals("email", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        string? phoneNumber = claims.FirstOrDefault(c => c.Type.Equals("phonenumber", StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty;
        // Result<User> result = await sender.Send(new CreateUser.CreateUserCommand(
        //     Id: null!,
        //     Username: username,
        //     PhoneNumber: phoneNumber,
        //     Email: email
        // ));
        User? checkingUser = await vitomDbContext.Users.SingleOrDefaultAsync(u => u.Id.Equals(id));
        if (checkingUser is null)
        {
            checkingUser = new()
            {
                Id = id,
                Username = username,
                PhoneNumber = phoneNumber,
                Email = email,
                Role = Domain.Enums.RolesEnum.Customer
            };
            vitomDbContext.Users.Add(checkingUser);
            await vitomDbContext.SaveChangesAsync(cancellationToken: default);
        }
        CurrentUser currentUser = context.RequestServices.GetRequiredService<CurrentUser>();
        currentUser.User = checkingUser;
        await next.Invoke(context);
    }
}