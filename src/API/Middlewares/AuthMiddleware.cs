using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Contracts;
using Domain.Entities;
using Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Middlewares;

public class AuthMiddleware(IVitomDbContext vitomDbContext) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // configure issuer
        string issuer = "https://exotic-squid-32.clerk.accounts.dev";
        string? authHeader = context.Request.Headers.Authorization.ToString();
        if (authHeader.IsNullOrEmpty() || !authHeader.Contains("Bearer "))
        {
            await next.Invoke(context);
            return;
        }
        // get token
        authHeader = authHeader.Replace("Bearer ", "");
        JwtSecurityTokenHandler? handler = new();
        JwtSecurityToken? jwtSecurityToken = handler.ReadJwtToken(authHeader);
        IEnumerable<Claim>? claims = jwtSecurityToken.Claims;
        if (!claims.Any(c => c.Issuer.Equals(issuer, StringComparison.InvariantCultureIgnoreCase)))
        {
            await next.Invoke(context);
            return;
        }
        // check user
        string? id =
            claims
                .FirstOrDefault(c =>
                    c.Type.Equals("sub", StringComparison.InvariantCultureIgnoreCase)
                )
                ?.Value ?? string.Empty;
        User? checkingUser = await vitomDbContext
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id.Equals(id));
        if (checkingUser is null)
        {
            checkingUser = new()
            {
                Id = id,
                Username = "admin",
                Email = "admin",
                PhoneNumber = "admin",
                ImageUrl = "admin",
                Role = Domain.Enums.RolesEnum.Admin,
                Cart = new() { UserId = id }
            };
            await vitomDbContext.Users.AddAsync(checkingUser);
            await vitomDbContext.SaveChangesAsync(cancellationToken: default);
        }
        CurrentUser currentUser = context.RequestServices.GetRequiredService<CurrentUser>();
        currentUser.User = checkingUser;
        await next.Invoke(context);
    }
}
