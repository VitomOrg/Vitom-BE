using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IFirebaseService
{
    Task<string> UploadFile(string name, IFormFile file);

    Task<string> UploadFile(string name, IFormFile file, string folderSave);

    Task<bool> DeleteFile(string imageUrl);
}