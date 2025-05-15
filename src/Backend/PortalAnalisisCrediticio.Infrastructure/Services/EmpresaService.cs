using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class EmpresaService : IEmpresaService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public EmpresaService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<EmpresaDTO> GetByIdAsync(int id)
    {
        var empresa = await _context.Empresas
            .Include(e => e.ClienteEmpresas)
                .ThenInclude(ce => ce.Cliente)
            .FirstOrDefaultAsync(e => e.Id == id);
        return _mapper.Map<EmpresaDTO>(empresa);
    }

    public async Task<IEnumerable<EmpresaDTO>> GetAllAsync()
    {
        var empresas = await _context.Empresas
            .Include(e => e.ClienteEmpresas)
                .ThenInclude(ce => ce.Cliente)
            .ToListAsync();
        return _mapper.Map<IEnumerable<EmpresaDTO>>(empresas);
    }

    public async Task<EmpresaDTO> CreateAsync(EmpresaDTO empresaDto)
    {
        var empresa = _mapper.Map<Empresa>(empresaDto);
        _context.Empresas.Add(empresa);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmpresaDTO>(empresa);
    }

    public async Task<EmpresaDTO> UpdateAsync(int id, EmpresaDTO empresaDto)
    {
        var empresa = await _context.Empresas.FindAsync(id);
        if (empresa == null)
            return null;
        _mapper.Map(empresaDto, empresa);
        await _context.SaveChangesAsync();
        return _mapper.Map<EmpresaDTO>(empresa);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var empresa = await _context.Empresas.FindAsync(id);
        if (empresa == null)
            return false;
        _context.Empresas.Remove(empresa);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ClienteDTO>> GetClientesAsync(int empresaId)
    {
        var empresa = await _context.Empresas
            .Include(e => e.ClienteEmpresas)
                .ThenInclude(ce => ce.Cliente)
            .FirstOrDefaultAsync(e => e.Id == empresaId);
        if (empresa == null)
            return Enumerable.Empty<ClienteDTO>();
        var clientes = empresa.ClienteEmpresas.Select(ce => ce.Cliente);
        return _mapper.Map<IEnumerable<ClienteDTO>>(clientes);
    }

    public async Task<ClienteEmpresaDTO> AsociarClienteAsync(int empresaId, int clienteId)
    {
        var empresa = await _context.Empresas.FindAsync(empresaId);
        var cliente = await _context.Clientes.FindAsync(clienteId);
        if (empresa == null || cliente == null)
            return null;
        var existe = await _context.ClienteEmpresas.AnyAsync(ce => ce.EmpresaId == empresaId && ce.ClienteId == clienteId);
        if (existe)
            return null;
        var clienteEmpresa = new ClienteEmpresa
        {
            EmpresaId = empresaId,
            ClienteId = clienteId,
            Activo = true,
            FechaInicio = DateTime.Now,
            Rol = "Asociado"
        };
        _context.ClienteEmpresas.Add(clienteEmpresa);
        await _context.SaveChangesAsync();
        return _mapper.Map<ClienteEmpresaDTO>(clienteEmpresa);
    }

    public async Task<bool> DesasociarClienteAsync(int empresaId, int clienteId)
    {
        var clienteEmpresa = await _context.ClienteEmpresas.FirstOrDefaultAsync(ce => ce.EmpresaId == empresaId && ce.ClienteId == clienteId);
        if (clienteEmpresa == null)
            return false;
        _context.ClienteEmpresas.Remove(clienteEmpresa);
        await _context.SaveChangesAsync();
        return true;
    }
} 