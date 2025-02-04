using Application.Contracts;
using Domain.ExternalEntities;
using Domain.Primitives;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.Cache;
using Infrastructure.Firebase;
using Infrastructure.Mail;
using Infrastructure.PayOSService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net.payOS;

namespace Infrastructure;

public static class Infrastructure_DI
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        // services.add;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
        });
        services.AddSingleton<ICacheServices, CacheServices>();
        services.AddScoped<IMailServices, MailServices>();
        services.AddScoped<IPayOSServices, PayOSServices>();
        services.AddDistributedMemoryCache();

        services.Configure<UrlSettings>(configuration.GetSection("UrlSettings"));
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        // register firebase storage
        services.AddSingleton<IFirebaseService>(s => new FirebaseStorageService(
            StorageClient.Create()
        ));

        FirebaseApp.Create(
            new AppOptions { Credential = GoogleCredential.FromFile("firebase.json") }
        );

        var fileName = "firebase.json";
        Environment.SetEnvironmentVariable(
            "GOOGLE_APPLICATION_CREDENTIALS",
            @Path.Combine(Environment.CurrentDirectory, fileName)
        );

        return services;
    }

    public static IServiceCollection AddPayOSService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        string clientId = configuration["ApiSettings:PayOS:ClientId"] ?? throw new Exception();
        string apiKey = configuration["ApiSettings:PayOS:ApiKey"] ?? throw new Exception();
        string checkSumKey =
            configuration["ApiSettings:PayOS:CheckSumKey"] ?? throw new Exception();

        PayOS payOS = new(clientId, apiKey, checkSumKey);
        services.AddScoped(s => payOS);

        return services;
    }
}