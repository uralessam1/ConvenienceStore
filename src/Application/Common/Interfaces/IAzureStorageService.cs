using Microsoft.AspNetCore.Http;

namespace ConvenienceStoreApi.Application.Common.Interfaces;

public interface IAzureStorageService
{
    Task<byte[]> DownloadFile(string Path);

    Task<bool> UploadFile(string fileName, string path, IFormFile file);
}