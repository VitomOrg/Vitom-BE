using Application.Contracts;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;

namespace Infrastructure.Firebase;

public class FirebaseStorageService(StorageClient storageClient) : IFirebaseService
{
    private readonly StorageClient _storageClient = storageClient;
    //the bucket name to save on firebase
    private const string BucketName = "vitom-firebase.appspot.com";
    private static readonly string[] separator = ["/o/"];

    public async Task<string> UploadFile(string fileName, IFormFile file)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        // Split the name into two parts: before and after the dot
        var nameParts = fileName.Split('.');
        var newName = $"{nameParts[0]}-{randomGuid}.{nameParts[1]}";

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var objectName = $"images/{newName}";
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

    public async Task<string> UploadFile(string fileName, IFormFile file, string folderSave)
    {
        // Create Guid to make the name of image unique
        var randomGuid = Guid.NewGuid();

        // Split the name into two parts: before and after the dot
        var nameParts = fileName.Split('.');
        var newName = $"{nameParts[0]}-{randomGuid}.{nameParts[1]}";

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        // Uploading image to FireBase
        var objectName = $"images/{folderSave}/{newName}";
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

    public async Task<string> UploadFiles(List<IFormFile> files, string folderSave)
    {
        var randomGuid = Guid.NewGuid();
        Stream[] fileStreams = files.Select(f => f.OpenReadStream()).ToArray();
        // Create a memory stream to hold the zip file
        MemoryStream zipMemoryStream = new();

        // Create the zip archive in the memory stream
        using (ZipArchive archive = new(zipMemoryStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < fileStreams.Length; i++)
            {
                // Add each file to the zip archive
                ZipArchiveEntry zipEntry = archive.CreateEntry(files[i].FileName);

                using (Stream entryStream = zipEntry.Open())
                {
                    // Copy the file stream to the zip entry
                    fileStreams[i].CopyTo(entryStream);
                }
            }
        }
        // Reset the position of the memory stream to the beginning before returning it
        zipMemoryStream.Seek(0, SeekOrigin.Begin);
        // Uploading image to FireBase
        var objectName = $"images/{folderSave}/DownloadProduct-{randomGuid}.zip";
        var image = await _storageClient.UploadObjectAsync(BucketName, objectName, "application/zip", zipMemoryStream);

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
}