using Application.Contracts;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Firebase;

public class FirebaseStorageService(StorageClient storageClient) : IFirebaseService
{
    private readonly StorageClient _storageClient = storageClient;
    //the bucket name to save on firebase
    private const string BucketName = "vitom-firebase.appspot.com";

    public async Task<string> UploadFile(string name, IFormFile file)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var image = await _storageClient.UploadObjectAsync(BucketName,
            $"images/ticket/{name}-{randomGuid}", file.ContentType, stream);

        //Get the image URI to get on the client the img
        var photoUri = image.MediaLink;

        return photoUri;
    }

    public async Task<string> UploadFile(string name, IFormFile file, string folderSave)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var image = await _storageClient.UploadObjectAsync(BucketName,
            $"images/{folderSave}/{name}-{randomGuid}", file.ContentType, stream);

        //Get the image URI to get on the client the img
        var photoUri = image.MediaLink;

        return photoUri;
    }
}