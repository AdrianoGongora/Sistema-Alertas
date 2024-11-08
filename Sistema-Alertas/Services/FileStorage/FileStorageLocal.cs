namespace Sistema_Alertas.Services.FileStorage;

public class FileStorageLocal : IFileStorageLocal
{
    public async Task<string> EditFile(IFormFile file, string route, string webRootPath, string scheme, string host)
    {
        await RemoveFile(route, webRootPath);

        return await SaveFile(file, webRootPath, scheme, host);
    }

    public Stream? GetFile(string fileName, string webRootPath)
    {
        var filePath = Path.Combine(webRootPath, fileName);

        if (!File.Exists(filePath))
        {
            return null;
        }

        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        return stream;
    }

    public Task RemoveFile(string route, string webRootPath)
    {
        if (string.IsNullOrWhiteSpace(route))
        {
            return Task.CompletedTask;
        }

        var fileName = Path.GetFileName(route);
        var directoryFile = Path.Combine(webRootPath, fileName);

        if (File.Exists(directoryFile))
        {
            File.Delete(directoryFile);
        }

        return Task.CompletedTask;
    }

    public async Task<string> SaveFile(IFormFile file, string webRootPath, string scheme, string host)
    {
        var extension = Path.GetExtension(webRootPath);
        var fileName = $"{Guid.NewGuid()}{extension}";

        var folder = Path.Combine(webRootPath);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string path = Path.Combine(folder, fileName);

        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var content = memoryStream.ToArray();
        await File.WriteAllBytesAsync(path, content);

        var currentUrl = $"{scheme}://{host}";
        var pathDb = Path.Combine(currentUrl, fileName).Replace("\\", "/");

        return pathDb;
    }

}