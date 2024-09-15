using Application.Contracts;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Infrastructure.Cache;
using Infrastructure.Firebase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class Infrastructure_DI
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // services.add;
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Cache");
        });
        services.AddSingleton<ICacheServices, CacheServices>();
        services.AddDistributedMemoryCache();

        // register firebase storage
        services.AddSingleton<IFirebaseService>(s => new FirebaseStorageService(StorageClient.Create()));

        FirebaseApp.Create(
            new AppOptions
            {
                Credential = GoogleCredential.FromFile("firebase.json")
            });

        var fileName = "firebase.json";
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @Path.Combine(Environment.CurrentDirectory, fileName));
        return services;
    }
}