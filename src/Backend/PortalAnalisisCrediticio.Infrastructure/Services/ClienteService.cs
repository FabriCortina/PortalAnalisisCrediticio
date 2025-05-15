using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class ClienteService : BaseService<Cliente, ClienteDTO>, IClienteService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClienteService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ClienteDTO> GetLegajoDigitalAsync(int id)
    {
        var cliente = await _dbSet
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(i => i.EstadosFinancieros)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(i => i.FlujosCajaProyectados)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(i => i.Deudas)
            .Include(c => c.ClienteEmpresas)
                .ThenInclude(ce => ce.Empresa)
            .FirstOrDefaultAsync(c => c.Id == id);

        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<ClienteDTO> ImportarDesdeExcelAsync(Stream fileStream)
    {
        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        var cliente = new Cliente
        {
            Nombre = worksheet.Cells[2, 1].Value?.ToString(),
            Apellido = worksheet.Cells[2, 2].Value?.ToString(),
            CUIT = worksheet.Cells[2, 3].Value?.ToString(),
            Email = worksheet.Cells[2, 4].Value?.ToString(),
            Telefono = worksheet.Cells[2, 5].Value?.ToString(),
            Direccion = worksheet.Cells[2, 6].Value?.ToString(),
            Ciudad = worksheet.Cells[2, 7].Value?.ToString(),
            Provincia = worksheet.Cells[2, 8].Value?.ToString(),
            Pais = worksheet.Cells[2, 9].Value?.ToString(),
            CodigoPostal = worksheet.Cells[2, 10].Value?.ToString(),
            FechaNacimiento = DateTime.Parse(worksheet.Cells[2, 11].Value?.ToString() ?? DateTime.Now.ToString()),
            EstadoCivil = worksheet.Cells[2, 12].Value?.ToString(),
            Ocupacion = worksheet.Cells[2, 13].Value?.ToString(),
            Nacionalidad = worksheet.Cells[2, 14].Value?.ToString(),
            TipoDocumento = worksheet.Cells[2, 15].Value?.ToString(),
            NumeroDocumento = worksheet.Cells[2, 16].Value?.ToString(),
            FechaAlta = DateTime.Now,
            Activo = true
        };

        await _dbSet.AddAsync(cliente);
        await _context.SaveChangesAsync();

        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<ClienteDTO> GetByIdWithDetailsAsync(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.EstadosFinancieros)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.FlujosCajaProyectados)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.Deudas)
            .Include(c => c.ClienteEmpresas)
                .ThenInclude(ce => ce.Empresa)
            .FirstOrDefaultAsync(c => c.Id == id);

        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<IEnumerable<ClienteDTO>> GetAllWithDetailsAsync()
    {
        var clientes = await _context.Clientes
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.EstadosFinancieros)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.FlujosCajaProyectados)
            .Include(c => c.InformacionFinanciera)
                .ThenInclude(f => f.Deudas)
            .Include(c => c.ClienteEmpresas)
                .ThenInclude(ce => ce.Empresa)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ClienteDTO>>(clientes);
    }

    public async Task<ClienteDTO> CreateWithDetailsAsync(ClienteDTO clienteDto)
    {
        var cliente = _mapper.Map<Cliente>(clienteDto);
        
        // Mapear la información financiera
        if (clienteDto.InformacionFinanciera != null)
        {
            cliente.InformacionFinanciera = _mapper.Map<InformacionFinanciera>(clienteDto.InformacionFinanciera);
        }

        // Mapear las empresas
        if (clienteDto.ClienteEmpresas != null)
        {
            cliente.ClienteEmpresas = _mapper.Map<ICollection<ClienteEmpresa>>(clienteDto.ClienteEmpresas);
        }

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<ClienteDTO> UpdateWithDetailsAsync(int id, ClienteDTO clienteDto)
    {
        var cliente = await _context.Clientes
            .Include(c => c.InformacionFinanciera)
            .Include(c => c.ClienteEmpresas)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (cliente == null)
            return null;

        _mapper.Map(clienteDto, cliente);

        // Actualizar la información financiera
        if (clienteDto.InformacionFinanciera != null)
        {
            if (cliente.InformacionFinanciera == null)
            {
                cliente.InformacionFinanciera = _mapper.Map<InformacionFinanciera>(clienteDto.InformacionFinanciera);
            }
            else
            {
                _mapper.Map(clienteDto.InformacionFinanciera, cliente.InformacionFinanciera);
            }
        }

        // Actualizar las empresas
        if (clienteDto.ClienteEmpresas != null)
        {
            cliente.ClienteEmpresas = _mapper.Map<ICollection<ClienteEmpresa>>(clienteDto.ClienteEmpresas);
        }

        await _context.SaveChangesAsync();

        return _mapper.Map<ClienteDTO>(cliente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _dbSet.FindAsync(id);
        if (cliente == null)
            return false;

        _dbSet.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }
} 