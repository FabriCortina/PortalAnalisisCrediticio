using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using OfficeOpenXml;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class ClienteService : BaseService<Cliente, ClienteDTO>, IClienteService
{
    private readonly ILogger<ClienteService> _logger;
    private readonly IMemoryCache _cache;
    private const int CACHE_DURATION_MINUTES = 60;

    public ClienteService(
        ApplicationDbContext context, 
        IMapper mapper,
        ILogger<ClienteService> logger,
        IMemoryCache cache) : base(context, mapper)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<IEnumerable<ClienteDTO>> GetAllAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todos los clientes");

            // Verificar caché
            var cacheKey = "clientes_all";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ClienteDTO> clientesCache))
            {
                _logger.LogInformation("Retornando clientes desde caché");
                return clientesCache;
            }

            var clientes = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);

            // Guardar en caché
            _cache.Set(cacheKey, dtos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Se obtuvieron {dtos.Count()} clientes exitosamente");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los clientes");
            throw new Exception("Error al obtener los clientes", ex);
        }
    }

    public async Task<IEnumerable<ClienteDTO>> GetAllWithDetailsAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todos los clientes con detalles");

            // Verificar caché
            var cacheKey = "clientes_all_details";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ClienteDTO> clientesCache))
            {
                _logger.LogInformation("Retornando clientes con detalles desde caché");
                return clientesCache;
            }

            var clientes = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                    .ThenInclude(ce => ce.Empresa)
                .Include(c => c.SolicitudesProducto)
                    .ThenInclude(sp => sp.Producto)
                .Include(c => c.InformesRiesgo)
                .Include(c => c.InformesExternos)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ClienteDTO>>(clientes);

            // Guardar en caché
            _cache.Set(cacheKey, dtos, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Se obtuvieron {dtos.Count()} clientes con detalles exitosamente");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los clientes con detalles");
            throw new Exception("Error al obtener los clientes con detalles", ex);
        }
    }

    public async Task<ClienteDTO> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Obteniendo cliente con ID {id}");

            // Verificar caché
            var cacheKey = $"cliente_{id}";
            if (_cache.TryGetValue(cacheKey, out ClienteDTO clienteCache))
            {
                _logger.LogInformation($"Retornando cliente {id} desde caché");
                return clienteCache;
            }

            var cliente = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {id} no encontrado");
                return null;
            }

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Guardar en caché
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Cliente {id} obtenido exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener cliente {id}");
            throw new Exception($"Error al obtener el cliente {id}", ex);
        }
    }

    public async Task<ClienteDTO> GetByIdWithDetailsAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Obteniendo cliente {id} con detalles");

            // Verificar caché
            var cacheKey = $"cliente_{id}_details";
            if (_cache.TryGetValue(cacheKey, out ClienteDTO clienteCache))
            {
                _logger.LogInformation($"Retornando cliente {id} con detalles desde caché");
                return clienteCache;
            }

            var cliente = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                    .ThenInclude(ce => ce.Empresa)
                .Include(c => c.SolicitudesProducto)
                    .ThenInclude(sp => sp.Producto)
                .Include(c => c.InformesRiesgo)
                .Include(c => c.InformesExternos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {id} no encontrado");
                return null;
            }

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Guardar en caché
            _cache.Set(cacheKey, dto, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation($"Cliente {id} con detalles obtenido exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al obtener cliente {id} con detalles");
            throw new Exception($"Error al obtener el cliente {id} con detalles", ex);
        }
    }

    public async Task<ClienteDTO> CreateAsync(ClienteDTO clienteDto)
    {
        try
        {
            _logger.LogInformation("Creando nuevo cliente");

            // Validar datos del cliente
            ValidarCliente(clienteDto);

            // Verificar CUIT duplicado
            var cuitExistente = await _dbSet.AnyAsync(c => c.CUIT == clienteDto.CUIT);
            if (cuitExistente)
            {
                _logger.LogWarning($"Ya existe un cliente con el CUIT {clienteDto.CUIT}");
                throw new Exception($"Ya existe un cliente con el CUIT {clienteDto.CUIT}");
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.FechaAlta = DateTime.UtcNow;
            cliente.Activo = true;

            await _dbSet.AddAsync(cliente);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Invalidar caché
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente {cliente.Id} creado exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente");
            throw new Exception("Error al crear el cliente", ex);
        }
    }

    public async Task<ClienteDTO> CreateWithDetailsAsync(ClienteDTO clienteDto)
    {
        try
        {
            _logger.LogInformation("Creando nuevo cliente con detalles");

            // Validar datos del cliente
            ValidarCliente(clienteDto);

            // Verificar CUIT duplicado
            var cuitExistente = await _dbSet.AnyAsync(c => c.CUIT == clienteDto.CUIT);
            if (cuitExistente)
            {
                _logger.LogWarning($"Ya existe un cliente con el CUIT {clienteDto.CUIT}");
                throw new Exception($"Ya existe un cliente con el CUIT {clienteDto.CUIT}");
            }

            var cliente = _mapper.Map<Cliente>(clienteDto);
            cliente.FechaAlta = DateTime.UtcNow;
            cliente.Activo = true;

            // Procesar relaciones
            if (clienteDto.ClienteEmpresas != null)
            {
                foreach (var ce in cliente.ClienteEmpresas)
                {
                    ce.FechaInicio = DateTime.UtcNow;
                    ce.Activo = true;
                }
            }

            await _dbSet.AddAsync(cliente);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Invalidar caché
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente {cliente.Id} creado con detalles exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente con detalles");
            throw new Exception("Error al crear el cliente con detalles", ex);
        }
    }

    public async Task<ClienteDTO> UpdateAsync(int id, ClienteDTO clienteDto)
    {
        try
        {
            _logger.LogInformation($"Actualizando cliente {id}");

            var cliente = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {id} no encontrado");
                return null;
            }

            // Validar datos del cliente
            ValidarCliente(clienteDto);

            // Verificar CUIT duplicado si cambió
            if (cliente.CUIT != clienteDto.CUIT)
            {
                var cuitExistente = await _dbSet.AnyAsync(c => c.CUIT == clienteDto.CUIT && c.Id != id);
                if (cuitExistente)
                {
                    _logger.LogWarning($"Ya existe otro cliente con el CUIT {clienteDto.CUIT}");
                    throw new Exception($"Ya existe otro cliente con el CUIT {clienteDto.CUIT}");
                }
            }

            _mapper.Map(clienteDto, cliente);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Invalidar caché
            _cache.Remove($"cliente_{id}");
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente {id} actualizado exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar cliente {id}");
            throw new Exception($"Error al actualizar el cliente {id}", ex);
        }
    }

    public async Task<ClienteDTO> UpdateWithDetailsAsync(int id, ClienteDTO clienteDto)
    {
        try
        {
            _logger.LogInformation($"Actualizando cliente {id} con detalles");

            var cliente = await _dbSet
                .Include(c => c.InformacionFinanciera)
                .Include(c => c.ClienteEmpresas)
                    .ThenInclude(ce => ce.Empresa)
                .Include(c => c.SolicitudesProducto)
                    .ThenInclude(sp => sp.Producto)
                .Include(c => c.InformesRiesgo)
                .Include(c => c.InformesExternos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {id} no encontrado");
                return null;
            }

            // Validar datos del cliente
            ValidarCliente(clienteDto);

            // Verificar CUIT duplicado si cambió
            if (cliente.CUIT != clienteDto.CUIT)
            {
                var cuitExistente = await _dbSet.AnyAsync(c => c.CUIT == clienteDto.CUIT && c.Id != id);
                if (cuitExistente)
                {
                    _logger.LogWarning($"Ya existe otro cliente con el CUIT {clienteDto.CUIT}");
                    throw new Exception($"Ya existe otro cliente con el CUIT {clienteDto.CUIT}");
                }
            }

            _mapper.Map(clienteDto, cliente);

            // Procesar relaciones
            if (clienteDto.ClienteEmpresas != null)
            {
                foreach (var ce in cliente.ClienteEmpresas)
                {
                    if (ce.FechaFin.HasValue && ce.Activo)
                    {
                        ce.Activo = false;
                    }
                }
            }

            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Invalidar caché
            _cache.Remove($"cliente_{id}");
            _cache.Remove($"cliente_{id}_details");
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente {id} actualizado con detalles exitosamente");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al actualizar cliente {id} con detalles");
            throw new Exception($"Error al actualizar el cliente {id} con detalles", ex);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation($"Eliminando cliente {id}");

            var cliente = await _dbSet.FindAsync(id);
            if (cliente == null)
            {
                _logger.LogWarning($"Cliente {id} no encontrado");
                return false;
            }

            // Verificar si tiene solicitudes activas
            var tieneSolicitudesActivas = await _context.SolicitudesProducto
                .AnyAsync(s => s.ClienteId == id && s.Estado == "Activo");

            if (tieneSolicitudesActivas)
            {
                _logger.LogWarning($"No se puede eliminar el cliente {id} porque tiene solicitudes activas");
                throw new Exception("No se puede eliminar el cliente porque tiene solicitudes activas");
            }

            _dbSet.Remove(cliente);
            await _context.SaveChangesAsync();

            // Invalidar caché
            _cache.Remove($"cliente_{id}");
            _cache.Remove($"cliente_{id}_details");
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente {id} eliminado exitosamente");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al eliminar cliente {id}");
            throw new Exception($"Error al eliminar el cliente {id}", ex);
        }
    }

    public async Task<ClienteDTO> ImportarDesdeExcelAsync(Stream fileStream)
    {
        try
        {
            _logger.LogInformation("Iniciando importación de cliente desde Excel");

            using var package = new ExcelPackage(fileStream);
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            if (rowCount < 2)
            {
                throw new Exception("El archivo Excel no contiene datos");
            }

            // Validar encabezados
            ValidarEncabezadosExcel(worksheet);

            var cuit = worksheet.Cells[2, 3].Value?.ToString();
            if (string.IsNullOrEmpty(cuit))
            {
                throw new Exception("El CUIT es requerido");
            }

            // Verificar CUIT duplicado
            var cuitExistente = await _dbSet.AnyAsync(c => c.CUIT == cuit);
            if (cuitExistente)
            {
                _logger.LogWarning($"Ya existe un cliente con el CUIT {cuit}");
                throw new Exception($"Ya existe un cliente con el CUIT {cuit}");
            }

            var cliente = new Cliente
            {
                Nombre = worksheet.Cells[2, 1].Value?.ToString(),
                Apellido = worksheet.Cells[2, 2].Value?.ToString(),
                CUIT = cuit,
                Email = worksheet.Cells[2, 4].Value?.ToString(),
                Telefono = worksheet.Cells[2, 5].Value?.ToString(),
                Direccion = worksheet.Cells[2, 6].Value?.ToString(),
                Ciudad = worksheet.Cells[2, 7].Value?.ToString(),
                Provincia = worksheet.Cells[2, 8].Value?.ToString(),
                Pais = worksheet.Cells[2, 9].Value?.ToString(),
                CodigoPostal = worksheet.Cells[2, 10].Value?.ToString(),
                FechaNacimiento = DateTime.TryParse(worksheet.Cells[2, 11].Value?.ToString(), out var fechaNac) ? fechaNac : DateTime.MinValue,
                EstadoCivil = worksheet.Cells[2, 12].Value?.ToString(),
                Ocupacion = worksheet.Cells[2, 13].Value?.ToString(),
                Nacionalidad = worksheet.Cells[2, 14].Value?.ToString(),
                TipoDocumento = worksheet.Cells[2, 15].Value?.ToString(),
                NumeroDocumento = worksheet.Cells[2, 16].Value?.ToString(),
                FechaAlta = DateTime.UtcNow,
                Activo = true
            };

            // Validar datos del cliente
            ValidarCliente(_mapper.Map<ClienteDTO>(cliente));

            await _dbSet.AddAsync(cliente);
            await _context.SaveChangesAsync();

            var dto = _mapper.Map<ClienteDTO>(cliente);

            // Invalidar caché
            _cache.Remove("clientes_all");
            _cache.Remove("clientes_all_details");

            _logger.LogInformation($"Cliente importado exitosamente desde Excel con ID {cliente.Id}");
            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al importar cliente desde Excel");
            throw new Exception("Error al importar cliente desde Excel", ex);
        }
    }

    private void ValidarCliente(ClienteDTO cliente)
    {
        if (string.IsNullOrEmpty(cliente.Nombre))
            throw new Exception("El nombre es requerido");

        if (string.IsNullOrEmpty(cliente.Apellido))
            throw new Exception("El apellido es requerido");

        if (string.IsNullOrEmpty(cliente.CUIT))
            throw new Exception("El CUIT es requerido");

        if (cliente.CUIT.Length != 11)
            throw new Exception("El CUIT debe tener 11 dígitos");

        if (!string.IsNullOrEmpty(cliente.Email) && !IsValidEmail(cliente.Email))
            throw new Exception("El email no es válido");

        if (!string.IsNullOrEmpty(cliente.Telefono) && !IsValidPhone(cliente.Telefono))
            throw new Exception("El teléfono no es válido");
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsValidPhone(string phone)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^\+?[0-9]{10,15}$");
    }

    private void ValidarEncabezadosExcel(ExcelWorksheet worksheet)
    {
        var headers = new[]
        {
            "Nombre",
            "Apellido",
            "CUIT",
            "Email",
            "Telefono",
            "Direccion",
            "Ciudad",
            "Provincia",
            "Pais",
            "CodigoPostal",
            "FechaNacimiento",
            "EstadoCivil",
            "Ocupacion",
            "Nacionalidad",
            "TipoDocumento",
            "NumeroDocumento"
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