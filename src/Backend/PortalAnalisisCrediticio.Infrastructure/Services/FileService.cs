using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.File;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _baseDirectory;
        private const int MAX_FILE_SIZE_MB = 10;
        private const int MAX_RETRY_ATTEMPTS = 3;
        private const int RETRY_DELAY_MS = 1000;
        private static readonly string[] ALLOWED_EXTENSIONS = { 
            ".pdf", ".doc", ".docx", ".xls", ".xlsx", 
            ".jpg", ".jpeg", ".png", ".txt", ".csv" 
        };
        private static readonly Dictionary<string, string> CONTENT_TYPES = new()
        {
            { ".pdf", "application/pdf" },
            { ".doc", "application/msword" },
            { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { ".xls", "application/vnd.ms-excel" },
            { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".png", "image/png" },
            { ".txt", "text/plain" },
            { ".csv", "text/csv" }
        };

        public FileService(
            ILogger<FileService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _baseDirectory = _configuration["FileStorage:BaseDirectory"] ?? "Uploads";

            EnsureDirectoryExists(_baseDirectory);
        }

        public async Task<FileDTO> GuardarArchivoAsync(Stream fileStream, string fileName, string contentType)
        {
            try
            {
                _logger.LogInformation($"Iniciando guardado de archivo: {fileName}");

                ValidateFile(fileStream, fileName, contentType);

                var uniqueFileName = GenerateUniqueFileName(fileName);
                var filePath = Path.Combine(_baseDirectory, uniqueFileName);

                await SaveFileWithRetryAsync(fileStream, filePath);

                var fileInfo = new FileInfo(filePath);
                var fileDto = new FileDTO
                {
                    Nombre = uniqueFileName,
                    NombreOriginal = fileName,
                    TipoContenido = contentType,
                    Tamaño = fileInfo.Length,
                    Ruta = filePath,
                    FechaCreacion = DateTime.UtcNow
                };

                _logger.LogInformation($"Archivo guardado exitosamente: {uniqueFileName}");
                return fileDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al guardar archivo: {fileName}");
                throw;
            }
        }

        public async Task<Stream> ObtenerArchivoAsync(string fileName)
        {
            try
            {
                _logger.LogInformation($"Iniciando obtención de archivo: {fileName}");

                var filePath = Path.Combine(_baseDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning($"Archivo no encontrado: {fileName}");
                    throw new FileNotFoundException($"El archivo {fileName} no existe");
                }

                var memoryStream = new MemoryStream();
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }
                memoryStream.Position = 0;

                _logger.LogInformation($"Archivo obtenido exitosamente: {fileName}");
                return memoryStream;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener archivo: {fileName}");
                throw;
            }
        }

        public async Task<bool> EliminarArchivoAsync(string fileName)
        {
            try
            {
                _logger.LogInformation($"Iniciando eliminación de archivo: {fileName}");

                var filePath = Path.Combine(_baseDirectory, fileName);
                if (!File.Exists(filePath))
                {
                    _logger.LogWarning($"Archivo no encontrado para eliminar: {fileName}");
                    return false;
                }

                await DeleteFileWithRetryAsync(filePath);
                _logger.LogInformation($"Archivo eliminado exitosamente: {fileName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar archivo: {fileName}");
                throw;
            }
        }

        public async Task<List<FileDTO>> ListarArchivosAsync(string directory = null)
        {
            try
            {
                _logger.LogInformation("Iniciando listado de archivos");

                var searchDirectory = string.IsNullOrEmpty(directory) 
                    ? _baseDirectory 
                    : Path.Combine(_baseDirectory, directory);

                if (!Directory.Exists(searchDirectory))
                {
                    _logger.LogWarning($"Directorio no encontrado: {searchDirectory}");
                    return new List<FileDTO>();
                }

                var files = Directory.GetFiles(searchDirectory);
                var fileDtos = new List<FileDTO>();

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    fileDtos.Add(new FileDTO
                    {
                        Nombre = fileInfo.Name,
                        NombreOriginal = fileInfo.Name,
                        TipoContenido = GetContentType(fileInfo.Extension),
                        Tamaño = fileInfo.Length,
                        Ruta = file,
                        FechaCreacion = fileInfo.CreationTimeUtc
                    });
                }

                _logger.LogInformation($"Se encontraron {fileDtos.Count} archivos");
                return fileDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar archivos");
                throw;
            }
        }

        public async Task<bool> CrearDirectorioAsync(string directoryName)
        {
            try
            {
                _logger.LogInformation($"Iniciando creación de directorio: {directoryName}");

                var directoryPath = Path.Combine(_baseDirectory, directoryName);
                if (Directory.Exists(directoryPath))
                {
                    _logger.LogWarning($"El directorio ya existe: {directoryName}");
                    return false;
                }

                Directory.CreateDirectory(directoryPath);
                _logger.LogInformation($"Directorio creado exitosamente: {directoryName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear directorio: {directoryName}");
                throw;
            }
        }

        public async Task<bool> EliminarDirectorioAsync(string directoryName)
        {
            try
            {
                _logger.LogInformation($"Iniciando eliminación de directorio: {directoryName}");

                var directoryPath = Path.Combine(_baseDirectory, directoryName);
                if (!Directory.Exists(directoryPath))
                {
                    _logger.LogWarning($"Directorio no encontrado: {directoryName}");
                    return false;
                }

                Directory.Delete(directoryPath, true);
                _logger.LogInformation($"Directorio eliminado exitosamente: {directoryName}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar directorio: {directoryName}");
                throw;
            }
        }

        private void ValidateFile(Stream fileStream, string fileName, string contentType)
        {
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("El archivo está vacío");
            }

            if (fileStream.Length > MAX_FILE_SIZE_MB * 1024 * 1024)
            {
                throw new ArgumentException($"El archivo excede el tamaño máximo permitido de {MAX_FILE_SIZE_MB}MB");
            }

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!ALLOWED_EXTENSIONS.Contains(extension))
            {
                throw new ArgumentException($"Tipo de archivo no permitido. Extensiones permitidas: {string.Join(", ", ALLOWED_EXTENSIONS)}");
            }

            var expectedContentType = GetContentType(extension);
            if (contentType != expectedContentType)
            {
                throw new ArgumentException($"El tipo de contenido no coincide con la extensión del archivo");
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var random = Guid.NewGuid().ToString("N").Substring(0, 8);
            return $"{fileNameWithoutExtension}_{timestamp}_{random}{extension}";
        }

        private async Task SaveFileWithRetryAsync(Stream fileStream, string filePath)
        {
            for (int attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++)
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await fileStream.CopyToAsync(stream);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Intento {attempt} de {MAX_RETRY_ATTEMPTS} fallido al guardar archivo: {filePath}");
                    
                    if (attempt == MAX_RETRY_ATTEMPTS)
                    {
                        throw;
                    }

                    await Task.Delay(RETRY_DELAY_MS * attempt);
                }
            }
        }

        private async Task DeleteFileWithRetryAsync(string filePath)
        {
            for (int attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++)
            {
                try
                {
                    File.Delete(filePath);
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, $"Intento {attempt} de {MAX_RETRY_ATTEMPTS} fallido al eliminar archivo: {filePath}");
                    
                    if (attempt == MAX_RETRY_ATTEMPTS)
                    {
                        throw;
                    }

                    await Task.Delay(RETRY_DELAY_MS * attempt);
                }
            }
        }

        private string GetContentType(string extension)
        {
            return CONTENT_TYPES.TryGetValue(extension.ToLowerInvariant(), out string contentType) 
                ? contentType 
                : "application/octet-stream";
        }

        private void EnsureDirectoryExists(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                _logger.LogInformation($"Directorio creado: {directory}");
            }
        }
    }
} 