// USING
using API.Extensions;
using API.Middlewares;
using Infrastructure;
using Application;
using Persistence;
using Serilog;
using Application.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
// builder config
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// using API Endpoints
builder.Services.AddEndpointsApiExplorer();
// using SWAGGER
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
});
// using PROJECTS
builder.Services.AddScoped<AuthMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(option =>
    {
        option.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "https://exotic-squid-32.clerk.accounts.dev";
        // options.Audience = audience;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false
        };
    });
builder.Services.AddAuthorization();
// using Custom Interfaces
builder.Services.AddScoped<IVitomDbContext, VitomDBContext>();
// using SERILOG
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
// app config
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option => option.DisplayRequestDuration());
    // Migrate Database
    app.MigrateDatabase<VitomDBContext>(async (dbContext, _) => await dbContext.Seed());
}
else
{
    app.MigrateDatabase<VitomDBContext>(async (dbContext, _) => await Task.CompletedTask);
}
app.UseCors();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseMiddleware<AuthMiddleware>();
app.MapMinimalAPI();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
