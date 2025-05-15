using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folder);
        Task<byte[]> GetFileAsync(string filePath);
        Task DeleteFileAsync(string filePath);
    }
} 