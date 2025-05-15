using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class InformacionFinancieraService : BaseService<InformacionFinanciera, InformacionFinancieraDTO>, IInformacionFinancieraService
{
    public InformacionFinancieraService(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<InformacionFinancieraDTO> GetByClienteIdAsync(int clienteId)
    {
        var informacionFinanciera = await _dbSet
            .Include(i => i.EstadosFinancieros)
            .Include(i => i.FlujosCajaProyectados)
            .Include(i => i.Deudas)
            .FirstOrDefaultAsync(i => i.ClienteId == clienteId);

        return _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);
    }

    public async Task<InformacionFinancieraDTO> CreateAsync(int clienteId, InformacionFinancieraDTO informacionDto)
    {
        var informacionFinanciera = _mapper.Map<InformacionFinanciera>(informacionDto);
        informacionFinanciera.ClienteId = clienteId;
        informacionFinanciera.FechaActualizacion = DateTime.Now;
        _dbSet.Add(informacionFinanciera);
        await _context.SaveChangesAsync();
        return _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);
    }

    public override async Task<InformacionFinancieraDTO> UpdateAsync(int id, InformacionFinancieraDTO informacionDto)
    {
        var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == id);
        if (informacionFinanciera == null)
            return null;
        _mapper.Map(informacionDto, informacionFinanciera);
        informacionFinanciera.FechaActualizacion = DateTime.Now;
        await _context.SaveChangesAsync();
        return _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);
    }

    public override async Task DeleteAsync(int id)
    {
        var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == id);
        if (informacionFinanciera != null)
        {
            _dbSet.Remove(informacionFinanciera);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<EstadoFinancieroDTO> AddEstadoFinancieroAsync(int clienteId, EstadoFinancieroDTO estadoDto)
    {
        var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
        if (informacionFinanciera == null)
            return null;
        var estadoFinanciero = _mapper.Map<EstadoFinanciero>(estadoDto);
        estadoFinanciero.InformacionFinancieraId = informacionFinanciera.Id;
        _context.EstadosFinancieros.Add(estadoFinanciero);
        await _context.SaveChangesAsync();
        return _mapper.Map<EstadoFinancieroDTO>(estadoFinanciero);
    }

    public async Task<FlujoCajaProyectadoDTO> AddFlujoCajaAsync(int clienteId, FlujoCajaProyectadoDTO flujoDto)
    {
        var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
        if (informacionFinanciera == null)
            return null;
        var flujoCaja = _mapper.Map<FlujoCajaProyectado>(flujoDto);
        flujoCaja.InformacionFinancieraId = informacionFinanciera.Id;
        _context.FlujosCajaProyectados.Add(flujoCaja);
        await _context.SaveChangesAsync();
        return _mapper.Map<FlujoCajaProyectadoDTO>(flujoCaja);
    }

    public async Task<DeudaDTO> AddDeudaAsync(int clienteId, DeudaDTO deudaDto)
    {
        var informacionFinanciera = await _dbSet.FirstOrDefaultAsync(i => i.ClienteId == clienteId);
        if (informacionFinanciera == null)
            return null;
        var deuda = _mapper.Map<Deuda>(deudaDto);
        deuda.InformacionFinancieraId = informacionFinanciera.Id;
        _context.Deudas.Add(deuda);
        await _context.SaveChangesAsync();
        return _mapper.Map<DeudaDTO>(deuda);
    }

    public async Task<InformacionFinancieraDTO> ImportarDesdeExcelAsync(Stream fileStream)
    {
        using var package = new ExcelPackage(fileStream);
        var worksheet = package.Workbook.Worksheets[0];
        var rowCount = worksheet.Dimension.Rows;

        var informacionFinanciera = new InformacionFinanciera
        {
            ClienteId = int.Parse(worksheet.Cells[2, 1].Value?.ToString() ?? "0"),
            IngresosMensuales = decimal.Parse(worksheet.Cells[2, 2].Value?.ToString() ?? "0"),
            GastosMensuales = decimal.Parse(worksheet.Cells[2, 3].Value?.ToString() ?? "0"),
            PatrimonioNeto = decimal.Parse(worksheet.Cells[2, 4].Value?.ToString() ?? "0"),
            Activos = decimal.Parse(worksheet.Cells[2, 5].Value?.ToString() ?? "0"),
            Pasivos = decimal.Parse(worksheet.Cells[2, 6].Value?.ToString() ?? "0"),
            Moneda = worksheet.Cells[2, 7].Value?.ToString(),
            FechaActualizacion = DateTime.Now,
            FuenteInformacion = "Excel",
            Observaciones = "Importado desde Excel"
        };

        await _dbSet.AddAsync(informacionFinanciera);
        await _context.SaveChangesAsync();

        return _mapper.Map<InformacionFinancieraDTO>(informacionFinanciera);
    }
} 