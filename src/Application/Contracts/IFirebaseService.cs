using Microsoft.AspNetCore.Http;

namespace Application.Contracts;

public interface IFirebaseService
{
    Task<string> UploadFile(string fileName, IFormFile file);

    Task<string> UploadFile(string fileName, IFormFile file, string folderSave);

    Task<bool> DeleteFile(string imageUrl);

    Task<string> UploadFiles(List<string> files, string folderSave);
}