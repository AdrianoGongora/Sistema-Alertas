namespace Sistema_Alertas.Services.FileStorage;

public interface IFileStorageLocal
{
    Task<string> SaveFile(IFormFile file, string webRootPath, string scheme, string host);
    Task<string> EditFile(IFormFile file, string route, string webRootPath, string scheme, string host);
    Task RemoveFile(string route, string webRootPath);
    Stream? GetFile(string fileName, string webRootPath);
}
