using Stopify.Services.Auth;

namespace Stopify.Services.Storage;

public class StorageService(HashingService hashingService) : IService
{
    public async Task<string> UploadFile(IFormFile file, string folderLocation)
    {
        Directory.CreateDirectory(folderLocation);

        var fileName = hashingService.GenerateUniqueFileNameHash(file);

        var filePath = Path.Combine(folderLocation, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, bufferSize: 4096, useAsync: true);

        await file.CopyToAsync(stream);

        return filePath;
    }
}