using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace ConvenienceStoreApi.Application.Common.Utils;

public static class ExtractFromFile
{
    public static async Task<string> ExtractTextFromFile(IFormFile file)
    {
        string fileContent;
        using var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8, true);
        fileContent = await reader.ReadToEndAsync();
        return fileContent;
    }

    public static async Task<string> SaveFile(IFormFile file, int ProductId)
    {

        string folderPath = @$"C:\Uploads\{ProductId}";
        /*   if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string fullPath = Path.Combine(folderPath, fileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return fullPath;
        */
        return folderPath;
    }
}