using Stopify.Services.Auth;

namespace Stopify.Services.Storage;

public class StorageService(HashingService hashingService) : IService
{
    public string UploadFile(IFormFile file, string folderLocation)
    {
        Directory.CreateDirectory(folderLocation);

        var fileName = hashingService.GenerateUniqueFileNameHash(file);

        var filePath = Path.Combine(folderLocation, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        file.CopyToAsync(stream);

        return filePath;
    }
}