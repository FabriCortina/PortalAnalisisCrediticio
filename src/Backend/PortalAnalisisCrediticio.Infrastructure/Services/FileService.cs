using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using PortalAnalisisCrediticio.Core.Interfaces;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory;

        public FileService(IConfiguration configuration)
        {
            _baseDirectory = configuration["FileStorage:BaseDirectory"] ?? "uploads";
            
            if (!Directory.Exists(_baseDirectory))
            {
                Directory.CreateDirectory(_baseDirectory);
            }
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo está vacío");

            var folderPath = Path.Combine(_baseDirectory, folder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<byte[]> GetFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("El archivo no existe");

            return await File.ReadAllBytesAsync(filePath);
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
            }
        }
    }
} 