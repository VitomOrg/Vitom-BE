// USINGasd
using API.Extensions;
using API.Middlewares;
using Infrastructure;
using Application;
using Persistence;
using Serilog;
using Application.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
// builder config
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// using API Endpoints
builder.Services.AddEndpointsApiExplorer();
// using SWAGGER
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
      {
        new OpenApiSecurityScheme
        {
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          },
        },
        Array.Empty<string>()
      }
    });
});
builder.Services.AddExceptionHandler<ValidationExceptionHandlerMiddleware>();
builder.Services.AddExceptionHandler<ExceptionHandlerMiddleware>();
builder.Services.AddProblemDetails();
// using PROJECTS
builder.Services.AddScoped<AuthMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration);
builder.Services.AddPersistence(configuration);
builder.Services.AddPayOSService(configuration);
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
            ValidateLifetime = false,
            ValidateIssuerSigningKey = false
        };
    });
builder.Services.AddAuthorization();
// using Custom Interfaces

//register validators
// builder.Services.AddScoped<IValidator<CreateProductEndpointHandler.CreateProductRequest>, CreateProductRequestValidator>();
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
    app.UseSwagger();
    app.UseSwaggerUI(option => option.DisplayRequestDuration());
    app.MigrateDatabase<VitomDBContext>(async (dbContext, _) => await dbContext.Seed());
}

await app.AddPayOS();
app.UseCors();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();
app.UseMiddleware<AuthMiddleware>();
app.MapMinimalAPI();

app.Run();
