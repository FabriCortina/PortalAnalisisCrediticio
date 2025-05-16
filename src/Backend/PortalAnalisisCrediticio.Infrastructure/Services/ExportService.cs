using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using ClosedXML.Excel;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Shared.DTOs.Dashboard;
using PortalAnalisisCrediticio.Shared.DTOs.Cliente;
using PortalAnalisisCrediticio.Shared.DTOs.Producto;
using PortalAnalisisCrediticio.Shared.DTOs.AnalisisRiesgo;

namespace PortalAnalisisCrediticio.Infrastructure.Services
{
    public class ExportService : IExportService
    {
        private readonly IDashboardService _dashboardService;
        private readonly IClienteService _clienteService;
        private readonly IAnalisisRiesgoService _analisisRiesgoService;
        private readonly ILogger<ExportService> _logger;
        private readonly IMemoryCache _cache;
        private const int CACHE_DURATION_MINUTES = 15;

        public ExportService(
            IDashboardService dashboardService,
            IClienteService clienteService,
            IAnalisisRiesgoService analisisRiesgoService,
            ILogger<ExportService> logger,
            IMemoryCache cache)
        {
            _dashboardService = dashboardService;
            _clienteService = clienteService;
            _analisisRiesgoService = analisisRiesgoService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<byte[]> ExportarCreditosActivosToExcelAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando créditos activos a Excel para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"excel_creditos_activos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo Excel desde caché");
                    return cachedFile;
                }

                var creditos = await _dashboardService.ObtenerCreditosActivosAsync(fechaInicio, fechaFin);
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Créditos Activos");

                // Configurar encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Cliente";
                worksheet.Cell(1, 3).Value = "Monto";
                worksheet.Cell(1, 4).Value = "Fecha Otorgamiento";
                worksheet.Cell(1, 5).Value = "Fecha Vencimiento";
                worksheet.Cell(1, 6).Value = "Estado";
                worksheet.Cell(1, 7).Value = "Saldo Actual";
                worksheet.Cell(1, 8).Value = "Tasa Interés";

                // Estilo para encabezados
                var headerRange = worksheet.Range(1, 1, 1, 8);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Agregar datos
                var row = 2;
                foreach (var credito in creditos)
                {
                    worksheet.Cell(row, 1).Value = credito.Id;
                    worksheet.Cell(row, 2).Value = credito.ClienteNombre;
                    worksheet.Cell(row, 3).Value = credito.Monto;
                    worksheet.Cell(row, 4).Value = credito.FechaOtorgamiento;
                    worksheet.Cell(row, 5).Value = credito.FechaVencimiento;
                    worksheet.Cell(row, 6).Value = credito.Estado;
                    worksheet.Cell(row, 7).Value = credito.SaldoActual;
                    worksheet.Cell(row, 8).Value = credito.TasaInteres;
                    row++;
                }

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Formatear números y fechas
                worksheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Column(8).Style.NumberFormat.Format = "0.00%";
                worksheet.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";
                worksheet.Column(5).Style.DateFormat.Format = "dd/MM/yyyy";

                // Agregar bordes
                var dataRange = worksheet.Range(1, 1, row - 1, 8);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo Excel generado exitosamente con {creditos.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar créditos activos a Excel");
                throw new Exception("Error al exportar créditos activos a Excel", ex);
            }
        }

        public async Task<byte[]> ExportarCreditosActivosToCsvAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando créditos activos a CSV para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"csv_creditos_activos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo CSV desde caché");
                    return cachedFile;
                }

                var creditos = await _dashboardService.ObtenerCreditosActivosAsync(fechaInicio, fechaFin);
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);

                // Escribir encabezados
                writer.WriteLine("ID,Cliente,Monto,Fecha Otorgamiento,Fecha Vencimiento,Estado,Saldo Actual,Tasa Interés");

                // Escribir datos
                foreach (var credito in creditos)
                {
                    writer.WriteLine(
                        $"{credito.Id}," +
                        $"\"{credito.ClienteNombre}\"," +
                        $"{credito.Monto:F2}," +
                        $"{credito.FechaOtorgamiento:yyyy-MM-dd}," +
                        $"{credito.FechaVencimiento:yyyy-MM-dd}," +
                        $"{credito.Estado}," +
                        $"{credito.SaldoActual:F2}," +
                        $"{credito.TasaInteres:P2}");
                }

                writer.Flush();
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo CSV generado exitosamente con {creditos.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar créditos activos a CSV");
                throw new Exception("Error al exportar créditos activos a CSV", ex);
            }
        }

        public async Task<byte[]> ExportarClientesRiesgoToExcelAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando clientes en riesgo a Excel para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"excel_clientes_riesgo_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo Excel desde caché");
                    return cachedFile;
                }

                var clientes = await _dashboardService.ObtenerClientesRiesgoAsync(fechaInicio, fechaFin);
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Clientes en Riesgo");

                // Configurar encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nombre";
                worksheet.Cell(1, 3).Value = "Tipo Cliente";
                worksheet.Cell(1, 4).Value = "Monto Total Deuda";
                worksheet.Cell(1, 5).Value = "Días Vencimiento";
                worksheet.Cell(1, 6).Value = "Nivel Riesgo";
                worksheet.Cell(1, 7).Value = "Score Crediticio";

                // Estilo para encabezados
                var headerRange = worksheet.Range(1, 1, 1, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Agregar datos
                var row = 2;
                foreach (var cliente in clientes)
                {
                    worksheet.Cell(row, 1).Value = cliente.Id;
                    worksheet.Cell(row, 2).Value = cliente.Nombre;
                    worksheet.Cell(row, 3).Value = cliente.TipoCliente;
                    worksheet.Cell(row, 4).Value = cliente.MontoTotalDeuda;
                    worksheet.Cell(row, 5).Value = cliente.DiasVencimiento;
                    worksheet.Cell(row, 6).Value = cliente.NivelRiesgo;
                    worksheet.Cell(row, 7).Value = cliente.ScoreCrediticio;
                    row++;
                }

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Formatear números
                worksheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Column(7).Style.NumberFormat.Format = "0.00";

                // Agregar bordes
                var dataRange = worksheet.Range(1, 1, row - 1, 7);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo Excel generado exitosamente con {clientes.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar clientes en riesgo a Excel");
                throw new Exception("Error al exportar clientes en riesgo a Excel", ex);
            }
        }

        public async Task<byte[]> ExportarClientesRiesgoToCsvAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando clientes en riesgo a CSV para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"csv_clientes_riesgo_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo CSV desde caché");
                    return cachedFile;
                }

                var clientes = await _dashboardService.ObtenerClientesRiesgoAsync(fechaInicio, fechaFin);
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);

                // Escribir encabezados
                writer.WriteLine("ID,Nombre,Tipo Cliente,Monto Total Deuda,Días Vencimiento,Nivel Riesgo,Score Crediticio");

                // Escribir datos
                foreach (var cliente in clientes)
                {
                    writer.WriteLine(
                        $"{cliente.Id}," +
                        $"\"{cliente.Nombre}\"," +
                        $"{cliente.TipoCliente}," +
                        $"{cliente.MontoTotalDeuda:F2}," +
                        $"{cliente.DiasVencimiento}," +
                        $"{cliente.NivelRiesgo}," +
                        $"{cliente.ScoreCrediticio:F2}");
                }

                writer.Flush();
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo CSV generado exitosamente con {clientes.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar clientes en riesgo a CSV");
                throw new Exception("Error al exportar clientes en riesgo a CSV", ex);
            }
        }

        public async Task<byte[]> ExportarCreditosVencidosToExcelAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando créditos vencidos a Excel para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"excel_creditos_vencidos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo Excel desde caché");
                    return cachedFile;
                }

                var creditos = await _dashboardService.ObtenerCreditosVencidosAsync(fechaInicio, fechaFin);
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Créditos Vencidos");

                // Configurar encabezados
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Cliente";
                worksheet.Cell(1, 3).Value = "Monto Vencido";
                worksheet.Cell(1, 4).Value = "Fecha Vencimiento";
                worksheet.Cell(1, 5).Value = "Días Vencimiento";
                worksheet.Cell(1, 6).Value = "Estado";
                worksheet.Cell(1, 7).Value = "Saldo Actual";

                // Estilo para encabezados
                var headerRange = worksheet.Range(1, 1, 1, 7);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Agregar datos
                var row = 2;
                foreach (var credito in creditos)
                {
                    worksheet.Cell(row, 1).Value = credito.Id;
                    worksheet.Cell(row, 2).Value = credito.ClienteNombre;
                    worksheet.Cell(row, 3).Value = credito.MontoVencido;
                    worksheet.Cell(row, 4).Value = credito.FechaVencimiento;
                    worksheet.Cell(row, 5).Value = credito.DiasVencimiento;
                    worksheet.Cell(row, 6).Value = credito.Estado;
                    worksheet.Cell(row, 7).Value = credito.SaldoActual;
                    row++;
                }

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Formatear números y fechas
                worksheet.Column(3).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Column(7).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Column(4).Style.DateFormat.Format = "dd/MM/yyyy";

                // Agregar bordes
                var dataRange = worksheet.Range(1, 1, row - 1, 7);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo Excel generado exitosamente con {creditos.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar créditos vencidos a Excel");
                throw new Exception("Error al exportar créditos vencidos a Excel", ex);
            }
        }

        public async Task<byte[]> ExportarCreditosVencidosToCsvAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando créditos vencidos a CSV para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"csv_creditos_vencidos_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo CSV desde caché");
                    return cachedFile;
                }

                var creditos = await _dashboardService.ObtenerCreditosVencidosAsync(fechaInicio, fechaFin);
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);

                // Escribir encabezados
                writer.WriteLine("ID,Cliente,Monto Vencido,Fecha Vencimiento,Días Vencimiento,Estado,Saldo Actual");

                // Escribir datos
                foreach (var credito in creditos)
                {
                    writer.WriteLine(
                        $"{credito.Id}," +
                        $"\"{credito.ClienteNombre}\"," +
                        $"{credito.MontoVencido:F2}," +
                        $"{credito.FechaVencimiento:yyyy-MM-dd}," +
                        $"{credito.DiasVencimiento}," +
                        $"{credito.Estado}," +
                        $"{credito.SaldoActual:F2}");
                }

                writer.Flush();
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation($"Archivo CSV generado exitosamente con {creditos.Count} registros");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar créditos vencidos a CSV");
                throw new Exception("Error al exportar créditos vencidos a CSV", ex);
            }
        }

        public async Task<byte[]> ExportarMetricasToExcelAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando métricas a Excel para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"excel_metricas_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo Excel desde caché");
                    return cachedFile;
                }

                var metricas = await _dashboardService.ObtenerMetricasAsync(fechaInicio, fechaFin);
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("Métricas");

                // Configurar encabezados
                worksheet.Cell(1, 1).Value = "Métrica";
                worksheet.Cell(1, 2).Value = "Valor";

                // Estilo para encabezados
                var headerRange = worksheet.Range(1, 1, 1, 2);
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Agregar datos
                var row = 2;
                worksheet.Cell(row, 1).Value = "Monto Total Créditos Activos";
                worksheet.Cell(row, 2).Value = metricas.MontoTotalCreditosActivos;
                row++;

                worksheet.Cell(row, 1).Value = "Cantidad Créditos Activos";
                worksheet.Cell(row, 2).Value = metricas.CantidadCreditosActivos;
                row++;

                worksheet.Cell(row, 1).Value = "Monto Total Créditos Vencidos";
                worksheet.Cell(row, 2).Value = metricas.MontoTotalCreditosVencidos;
                row++;

                worksheet.Cell(row, 1).Value = "Cantidad Créditos Vencidos";
                worksheet.Cell(row, 2).Value = metricas.CantidadCreditosVencidos;
                row++;

                worksheet.Cell(row, 1).Value = "Tasa de Mora";
                worksheet.Cell(row, 2).Value = metricas.TasaMora;
                row++;

                worksheet.Cell(row, 1).Value = "Cantidad Clientes Riesgo";
                worksheet.Cell(row, 2).Value = metricas.CantidadClientesRiesgo;
                row++;

                worksheet.Cell(row, 1).Value = "Monto Total Recuperado";
                worksheet.Cell(row, 2).Value = metricas.MontoTotalRecuperado;
                row++;

                worksheet.Cell(row, 1).Value = "Cantidad Nuevos Clientes";
                worksheet.Cell(row, 2).Value = metricas.CantidadNuevosClientes;

                // Ajustar columnas
                worksheet.Columns().AdjustToContents();

                // Formatear números
                worksheet.Column(2).Style.NumberFormat.Format = "#,##0.00";
                worksheet.Cell(6, 2).Style.NumberFormat.Format = "0.00%";

                // Agregar bordes
                var dataRange = worksheet.Range(1, 1, row, 2);
                dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation("Archivo Excel de métricas generado exitosamente");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar métricas a Excel");
                throw new Exception("Error al exportar métricas a Excel", ex);
            }
        }

        public async Task<byte[]> ExportarMetricasToCsvAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                _logger.LogInformation($"Exportando métricas a CSV para el período {fechaInicio:yyyy-MM-dd} a {fechaFin:yyyy-MM-dd}");

                var cacheKey = $"csv_metricas_{fechaInicio:yyyyMMdd}_{fechaFin:yyyyMMdd}";
                if (_cache.TryGetValue(cacheKey, out byte[] cachedFile))
                {
                    _logger.LogInformation("Retornando archivo CSV desde caché");
                    return cachedFile;
                }

                var metricas = await _dashboardService.ObtenerMetricasAsync(fechaInicio, fechaFin);
                using var stream = new MemoryStream();
                using var writer = new StreamWriter(stream);

                // Escribir encabezados
                writer.WriteLine("Métrica,Valor");

                // Escribir datos
                writer.WriteLine($"Monto Total Créditos Activos,{metricas.MontoTotalCreditosActivos:F2}");
                writer.WriteLine($"Cantidad Créditos Activos,{metricas.CantidadCreditosActivos}");
                writer.WriteLine($"Monto Total Créditos Vencidos,{metricas.MontoTotalCreditosVencidos:F2}");
                writer.WriteLine($"Cantidad Créditos Vencidos,{metricas.CantidadCreditosVencidos}");
                writer.WriteLine($"Tasa de Mora,{metricas.TasaMora:P2}");
                writer.WriteLine($"Cantidad Clientes Riesgo,{metricas.CantidadClientesRiesgo}");
                writer.WriteLine($"Monto Total Recuperado,{metricas.MontoTotalRecuperado:F2}");
                writer.WriteLine($"Cantidad Nuevos Clientes,{metricas.CantidadNuevosClientes}");

                writer.Flush();
                var fileBytes = stream.ToArray();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

                _cache.Set(cacheKey, fileBytes, cacheOptions);
                _logger.LogInformation("Archivo CSV de métricas generado exitosamente");

                return fileBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al exportar métricas a CSV");
                throw new Exception("Error al exportar métricas a CSV", ex);
            }
        }
    }
} 