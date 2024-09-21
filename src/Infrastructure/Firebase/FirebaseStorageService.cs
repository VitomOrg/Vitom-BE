using Application.Contracts;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Firebase;

public class FirebaseStorageService(StorageClient storageClient) : IFirebaseService
{
    private readonly StorageClient _storageClient = storageClient;
    //the bucket name to save on firebase
    private const string BucketName = "vitom-firebase.appspot.com";
    private static readonly string[] separator = ["/o/"];

    public async Task<string> UploadFile(string name, IFormFile file)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var objectName = $"images/{name}-{randomGuid}";
        var image = await _storageClient.UploadObjectAsync(BucketName, objectName, file.ContentType, stream);

        // Make the object publicly accessible
        await _storageClient.UpdateObjectAsync(new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = BucketName,
            Name = objectName,
            Acl =
            [
                new Google.Apis.Storage.v1.Data.ObjectAccessControl
                {
                    Entity = "allUsers",
                    Role = "READER"
                }
            ]
        });

        // Construct and return the public URL for the uploaded image
        var publicUrl = $"https://storage.googleapis.com/{BucketName}/{objectName}";

        return publicUrl;
    }

    public async Task<string> UploadFile(string name, IFormFile file, string folderSave)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var objectName = $"images/{folderSave}/{name}-{randomGuid}";
        var image = await _storageClient.UploadObjectAsync(BucketName, objectName, file.ContentType, stream);

        // Make the object publicly accessible
        await _storageClient.UpdateObjectAsync(new Google.Apis.Storage.v1.Data.Object
        {
            Bucket = BucketName,
            Name = objectName,
            Acl =
            [
                new Google.Apis.Storage.v1.Data.ObjectAccessControl
                {
                    Entity = "allUsers",
                    Role = "READER"
                }
            ]
        });

        // Construct and return the public URL for the uploaded image
        var publicUrl = $"https://storage.googleapis.com/{BucketName}/{objectName}";

        return publicUrl;
    }

    public async Task<bool> DeleteFile(string imageUrl)
    {
        try
        {
            // Extract the file path from the URL (after the bucket name)
            var storageBaseUrl = $"https://storage.googleapis.com/{BucketName}/";

            // Ensure the URL is valid and belongs to the expected bucket
            if (!imageUrl.StartsWith(storageBaseUrl))
                return false;

            // Extract the object name from the URL
            var objectName = imageUrl.Substring(storageBaseUrl.Length);

            // Delete the object from Firebase Storage
            await _storageClient.DeleteObjectAsync(BucketName, objectName);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}