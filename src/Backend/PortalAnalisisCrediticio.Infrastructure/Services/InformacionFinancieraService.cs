using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class InformacionFinancieraService : BaseService<InformacionFinanciera, InformacionFinancieraDTO>, IInformacionFinancieraService
{
    private readonly ILogger<InformacionFinancieraService> _logger;
    private readonly IMemoryCache _cache;
    private const int CACHE_DURATION_MINUTES = 60;

    public InformacionFinancieraService(
        ApplicationDbContext context, 
        IMapper mapper,
        ILogger<InformacionFinancieraService> logger,
        IMemoryCache cache) : base(context, mapper)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<InformacionFinancieraDTO> GetByClienteIdAsync(int clienteId)
    {
        try
        {
            _logger.LogInformation($"Obteniendo información financiera para el cliente {clienteId}");

            // Verificar caché
            var cacheKey = $"info_financiera_{clienteId}";
            if (_cache.TryGetValue(cacheKey, out InformacionFinancieraDTO infoCache))
            {
                _logger.LogInformation($"Retornando información financiera desde caché para el cliente {clienteId}");
                return infoCache;
            }

            var informacionFinanciera = await _dbSet
                .Include(i => i.EstadosFinancieros)
                .Include(i => i.FlujosCajaProyectados)
                .Include(i => i.Deudas)
                .FirstOrDefaultAsync(i => i.ClienteId == clienteId);

            if (informacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {clienteId}");
                return null;
            }

            var dto = _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);

            // Guardar en caché
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Información financiera obtenida exitosamente para el cliente {clienteId}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener información financiera para el cliente {clienteId}");
            throw new Exception($"Error al obtener información financiera: {ex.Message}", ex);
        }
    }

    public async Task<InformacionFinancieraDTO> CreateAsync(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        try
        {
            _logger.LogInformation($"Creando información financiera para el cliente {clienteId}");

            // Validar cliente
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {clienteId} no encontrado");
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Validar información financiera existente
            var infoExistente = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
            if (infoExistente != null)
            {
                _logger.LogWarning($"Ya existe información financiera para el cliente {clienteId}");
                throw new Exception("Ya existe información financiera para este cliente");
            }

            var informacionFinanciera = _mapper.Map<InformacionFinanciera>(informacionDto);
            informacionFinanciera.ClienteId = clienteId;
            informacionFinanciera.FechaActualizacion = DateTime.UtcNow;

            // Validar datos financieros
            ValidarDatosFinancieros(informacionFinanciera);

            _dbSet.Add(informacionFinanciera);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);

            // Actualizar caché
            var cacheKey = $"info_financiera_{clienteId}";
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Información financiera creada exitosamente para el cliente {clienteId}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al crear información financiera para el cliente {clienteId}");
            throw new Exception($"Error al crear información financiera: {ex.Message}", ex);
        }
    }

    public override async Task<InformacionFinancieraDTO> UpdateAsync(int id, InformacionFinancieraDTO informacionDto)
    {
        try
        {
            _logger.LogInformation($"Actualizando información financiera para el cliente {id}");

            var informacionFinanciera = await _dbSet
                .Include(i => i.EstadosFinancieros)
                .Include(i => i.FlujosCajaProyectados)
                .Include(i => i.Deudas)
                .FirstOrDefaultAsync(i => i.ClienteId == id);

            if (informacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {id}");
                return null;
            }

            _mapper.Map(informacionDto, informacionFinanciera);
            informacionFinanciera.FechaActualizacion = DateTime.UtcNow;

            // Validar datos financieros
            ValidarDatosFinancieros(informacionFinanciera);

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);

            // Actualizar caché
            var cacheKey = $"info_financiera_{id}";
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Información financiera actualizada exitosamente para el cliente {id}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar información financiera para el cliente {id}");
            throw new Exception($"Error al actualizar información financiera: {ex.Message}", ex);
        }
    }

    public override async Task DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Eliminando información financiera para el cliente {id}");

            var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == id);
            if (informacionFinanciera != null)
            {
                _dbSet.Remove(informacionFinanciera);
                await _context.SaveChangesAsync();

                // Eliminar de caché
                var cacheKey = $"info_financiera_{id}";
                _cache.Remove(cacheKey);

                _logger.LogInformation($"Información financiera eliminada exitosamente para el cliente {id}");
            }
            else
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {id}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar información financiera para el cliente {id}");
            throw new Exception($"Error al eliminar información financiera: {ex.Message}", ex);
        }
    }

    public async Task<EstadoFinancieroDTO> AddEstadoFinancieroAsync(int clienteId, EstadoFinancieroDTO estadoDto)
    {
        try
        {
            _logger.LogInformation($"Agregando estado financiero para el cliente {clienteId}");

            var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
            if (informacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {clienteId}");
                return null;
            }

            var estadoFinanciero = _mapper.Map<EstadoFinanciero>(estadoDto);
            estadoFinanciero.InformacionFinancieraId = informacionFinanciera.Id;
            estadoFinanciero.Fecha = DateTime.UtcNow;

            // Validar estado financiero
            ValidarEstadoFinanciero(estadoFinanciero);

            _context.EstadosFinancieros.Add(estadoFinanciero);
            await _context.SaveChangesAsync();

            // Invalidar caché
            var cacheKey = $"info_financiera_{clienteId}";
            _cache.Remove(cacheKey);

            _logger.LogInformation($"Estado financiero agregado exitosamente para el cliente {clienteId}");
            return _mapper.Map<EstadoFinancieroDTO>(estadoFinanciero);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al agregar estado financiero para el cliente {clienteId}");
            throw new Exception($"Error al agregar estado financiero: {ex.Message}", ex);
        }
    }

    public async Task<FlujoCajaProyectadoDTO> AddFlujoCajaAsync(int clienteId, FlujoCajaProyectadoDTO flujoDto)
    {
        try
        {
            _logger.LogInformation($"Agregando flujo de caja para el cliente {clienteId}");

            var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
            if (informacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {clienteId}");
                return null;
            }

            var flujoCaja = _mapper.Map<FlujoCajaProyectado>(flujoDto);
            flujoCaja.InformacionFinancieraId = informacionFinanciera.Id;
            flujoCaja.Fecha = DateTime.UtcNow;

            // Validar flujo de caja
            ValidarFlujoCaja(flujoCaja);

            _context.FlujosCajaProyectados.Add(flujoCaja);
            await _context.SaveChangesAsync();

            // Invalidar caché
            var cacheKey = $"info_financiera_{clienteId}";
            _cache.Remove(cacheKey);

            _logger.LogInformation($"Flujo de caja agregado exitosamente para el cliente {clienteId}");
            return _mapper.Map<FlujoCajaProyectadoDTO>(flujoCaja);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al agregar flujo de caja para el cliente {clienteId}");
            throw new Exception($"Error al agregar flujo de caja: {ex.Message}", ex);
        }
    }

    public async Task<DeudaDTO> AddDeudaAsync(int clienteId, DeudaDTO deudaDto)
    {
        try
        {
            _logger.LogInformation($"Agregando deuda para el cliente {clienteId}");

            var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
            if (informacionFinanciera == null)
            {
                _logger.LogWarning($"Información financiera no encontrada para el cliente {clienteId}");
                return null;
            }

            var deuda = _mapper.Map<Deuda>(deudaDto);
            deuda.InformacionFinancieraId = informacionFinanciera.Id;
            deuda.Fecha = DateTime.UtcNow;

            // Validar deuda
            ValidarDeuda(deuda);

            _context.Deudas.Add(deuda);
            await _context.SaveChangesAsync();

            // Invalidar caché
            var cacheKey = $"info_financiera_{clienteId}";
            _cache.Remove(cacheKey);

            _logger.LogInformation($"Deuda agregada exitosamente para el cliente {clienteId}");
            return _mapper.Map<DeudaDTO>(deuda);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al agregar deuda para el cliente {clienteId}");
            throw new Exception($"Error al agregar deuda: {ex.Message}", ex);
        }
    }

    public async Task<InformacionFinancieraDTO> ImportarDesdeExcelAsync(Stream fileStream)
    {
        try
        {
            _logger.LogInformation("Iniciando importación de información financiera desde Excel");

            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            if (rowCount < 2)
            {
                throw new Exception("El archivo Excel no contiene datos");
            }

            // Validar encabezados
            ValidarEncabezadosExcel(worksheet);

            var clienteId = int.Parse(worksheet.Cells[2, 1].Value?.ToString() ?? "0");
            if (clienteId <= 0)
            {
                throw new Exception("ID de cliente inválido");
            }

            // Validar cliente existente
            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                throw new Exception($"Cliente {clienteId} no encontrado");
            }

            // Validar información financiera existente
            var infoExistente = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
            if (infoExistente != null)
            {
                throw new Exception("Ya existe información financiera para este cliente");
            }

            var informacionFinanciera = new InformacionFinanciera
            {
                ClienteId = clienteId,
                IngresosMensuales = decimal.Parse(worksheet.Cells[2, 2].Value?.ToString() ?? "0"),
                GastosMensuales = decimal.Parse(worksheet.Cells[2, 3].Value?.ToString() ?? "0"),
                PatrimonioNeto = decimal.Parse(worksheet.Cells[2, 4].Value?.ToString() ?? "0"),
                Activos = decimal.Parse(worksheet.Cells[2, 5].Value?.ToString() ?? "0"),
                Pasivos = decimal.Parse(worksheet.Cells[2, 6].Value?.ToString() ?? "0"),
                Moneda = worksheet.Cells[2, 7].Value?.ToString() ?? "ARS",
                FechaActualizacion = DateTime.UtcNow,
                FuenteInformacion = "Excel",
                Observaciones = "Importado desde Excel"
            };

            // Validar datos financieros
            ValidarDatosFinancieros(informacionFinanciera);

            await _dbSet.AddAsync(informacionFinanciera);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);

            // Actualizar caché
            var cacheKey = $"info_financiera_{clienteId}";
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Información financiera importada exitosamente desde Excel para el cliente {clienteId}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al importar información financiera desde Excel");
            throw new Exception($"Error al importar información financiera: {ex.Message}", ex);
        }
    }

    private void ValidarDatosFinancieros(InformacionFinanciera informacion)
    {
        if (informacion.IngresosMensuales < 0)
            throw new Exception("Los ingresos mensuales no pueden ser negativos");

        if (informacion.GastosMensuales < 0)
            throw new Exception("Los gastos mensuales no pueden ser negativos");

        if (informacion.PatrimonioNeto < 0)
            throw new Exception("El patrimonio neto no puede ser negativo");

        if (informacion.Activos < 0)
            throw new Exception("Los activos no pueden ser negativos");

        if (informacion.Pasivos < 0)
            throw new Exception("Los pasivos no pueden ser negativos");

        if (string.IsNullOrEmpty(informacion.Moneda))
            throw new Exception("La moneda es requerida");
    }

    private void ValidarEstadoFinanciero(EstadoFinanciero estado)
    {
        if (string.IsNullOrEmpty(estado.Tipo))
            throw new Exception("El tipo de estado financiero es requerido");

        if (estado.Ventas < 0)
            throw new Exception("Las ventas no pueden ser negativas");

        if (estado.CostoVentas < 0)
            throw new Exception("El costo de ventas no puede ser negativo");

        if (estado.GastosOperativos < 0)
            throw new Exception("Los gastos operativos no pueden ser negativos");

        if (estado.ActivosCorrientes < 0)
            throw new Exception("Los activos corrientes no pueden ser negativos");

        if (estado.ActivosNoCorrientes < 0)
            throw new Exception("Los activos no corrientes no pueden ser negativos");

        if (estado.PasivosCorrientes < 0)
            throw new Exception("Los pasivos corrientes no pueden ser negativos");

        if (estado.PasivosNoCorrientes < 0)
            throw new Exception("Los pasivos no corrientes no pueden ser negativos");

        if (estado.Capital < 0)
            throw new Exception("El capital no puede ser negativo");
    }

    private void ValidarFlujoCaja(FlujoCajaProyectado flujo)
    {
        if (flujo.Fecha < DateTime.UtcNow)
            throw new Exception("La fecha del flujo de caja no puede ser anterior a la fecha actual");

        if (flujo.Ingresos < 0)
            throw new Exception("Los ingresos no pueden ser negativos");

        if (flujo.Egresos < 0)
            throw new Exception("Los egresos no pueden ser negativos");

        if (flujo.Saldo < 0)
            throw new Exception("El saldo no puede ser negativo");
    }

    private void ValidarDeuda(Deuda deuda)
    {
        if (string.IsNullOrEmpty(deuda.Tipo))
            throw new Exception("El tipo de deuda es requerido");

        if (deuda.Monto < 0)
            throw new Exception("El monto de la deuda no puede ser negativo");

        if (deuda.TasaInteres < 0)
            throw new Exception("La tasa de interés no puede ser negativa");

        if (deuda.Plazo < 0)
            throw new Exception("El plazo no puede ser negativo");
    }

    private void ValidarEncabezadosExcel(ExcelWorksheet worksheet)
    {
        var headers = new[]
        {
            "ClienteId",
            "IngresosMensuales",
            "GastosMensuales",
            "PatrimonioNeto",
            "Activos",
            "Pasivos",
            "Moneda"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            var header = worksheet.Cells[1, i + 1].Value?.ToString();
            if (header != headers[i])
            {
                throw new Exception($"Encabezado inválido en la columna {i + 1}. Se esperaba '{headers[i]}' pero se encontró '{header}'");
            }
        }
    }
} 