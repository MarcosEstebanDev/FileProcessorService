using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace FileProcessorService.Services
{
    public class FileProcessorService
    {
        private readonly ILogger<FileProcessorService> _logger;
        private readonly string _watchDirectory = @"C:\ArchivosAProcesar"; // 📂 Ruta a monitorear

        public FileProcessorService(ILogger<FileProcessorService> logger)
        {
            _logger = logger;
        }

        public Task ProcessFiles()
        {
            _logger.LogInformation("🔍 Buscando archivos en {Directory}", _watchDirectory);

            if (!Directory.Exists(_watchDirectory))
            {
                _logger.LogWarning("⚠️ La carpeta {Directory} no existe", _watchDirectory);
                return Task.CompletedTask;
            }

            var files = Directory.GetFiles(_watchDirectory, "*.txt"); // Solo archivos .txt
            foreach (var file in files)
            {
                _logger.LogInformation("📂 Procesando archivo: {FileName}", file);
                // Aquí iría la lógica para procesar el archivo...
                File.Move(file, Path.Combine(_watchDirectory, "Procesados", Path.GetFileName(file)));
            }

            return Task.CompletedTask;
        }
    }
}
