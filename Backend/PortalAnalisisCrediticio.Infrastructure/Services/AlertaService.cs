using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Domain.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Alerta;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class AlertaService : IAlertaService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AlertaService> _logger;
    private readonly IEmailService _emailService;

    public AlertaService(ApplicationDbContext context, ILogger<AlertaService> logger, IEmailService emailService)
    {
        _context = context;
        _logger = logger;
        _emailService = emailService;
    }

    public async Task<IEnumerable<AlertaDTO>> GetAllAsync()
    {
        var alertas = await _context.Alertas.ToListAsync();
        return alertas.Select(MapToDTO);
    }

    public async Task<AlertaDTO> GetByIdAsync(int id)
    {
        var alerta = await _context.Alertas.FindAsync(id);
        return alerta == null ? null : MapToDTO(alerta);
    }

    public async Task<IEnumerable<AlertaDTO>> GetByClienteIdAsync(int clienteId)
    {
        var alertas = await _context.Alertas.Where(a => a.ClienteId == clienteId).ToListAsync();
        return alertas.Select(MapToDTO);
    }

    public async Task<AlertaDTO> CreateAsync(CreateAlertaDTO dto)
    {
        var alerta = new Alerta
        {
            ClienteId = dto.ClienteId,
            TipoEvento = dto.TipoEvento,
            MedioNotificacion = dto.MedioNotificacion,
            DiasAntesVencimiento = dto.DiasAntesVencimiento,
            EstadoFinancieroObjetivo = dto.EstadoFinancieroObjetivo,
            Activa = dto.Activa,
            EmailDestino = dto.EmailDestino,
            FechaCreacion = DateTime.UtcNow
        };
        _context.Alertas.Add(alerta);
        await _context.SaveChangesAsync();
        return MapToDTO(alerta);
    }

    public async Task<AlertaDTO> UpdateAsync(int id, UpdateAlertaDTO dto)
    {
        var alerta = await _context.Alertas.FindAsync(id);
        if (alerta == null) return null;
        alerta.MedioNotificacion = dto.MedioNotificacion;
        alerta.DiasAntesVencimiento = dto.DiasAntesVencimiento;
        alerta.EstadoFinancieroObjetivo = dto.EstadoFinancieroObjetivo;
        alerta.Activa = dto.Activa;
        alerta.EmailDestino = dto.EmailDestino;
        await _context.SaveChangesAsync();
        return MapToDTO(alerta);
    }

    public async Task DeleteAsync(int id)
    {
        var alerta = await _context.Alertas.FindAsync(id);
        if (alerta != null)
        {
            _context.Alertas.Remove(alerta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ProcesarAlertasAsync()
    {
        _logger.LogInformation("Procesando alertas...");
        var alertas = await _context.Alertas.Where(a => a.Activa).ToListAsync();
        foreach (var alerta in alertas)
        {
            if (alerta.TipoEvento == "VencimientoCuota")
            {
                await EnviarNotificacionVencimiento(alerta);
            }
            else if (alerta.TipoEvento == "CambioEstadoFinanciero")
            {
                await EnviarNotificacionCambioEstadoFinanciero(alerta);
            }
        }
    }

    private async Task EnviarNotificacionVencimiento(Alerta alerta)
    {
        _logger.LogInformation("Enviando notificación de vencimiento de cuota para alerta {AlertaId}", alerta.Id);
        var destinatario = alerta.EmailDestino ?? "destinatario@ejemplo.com"; // Fallback a un email por defecto
        var asunto = "Alerta: Vencimiento de cuota";
        var mensaje = $"Se le informa que tiene una cuota próxima a vencer. Alerta ID: {alerta.Id}";
        await _emailService.EnviarEmailAsync(destinatario, asunto, mensaje);
    }

    private async Task EnviarNotificacionCambioEstadoFinanciero(Alerta alerta)
    {
        _logger.LogInformation("Enviando notificación de cambio de estado financiero para alerta {AlertaId}", alerta.Id);
        var destinatario = alerta.EmailDestino ?? "destinatario@ejemplo.com"; // Fallback a un email por defecto
        var asunto = "Alerta: Cambio de estado financiero";
        var mensaje = $"Se le informa que ha ocurrido un cambio en su estado financiero. Alerta ID: {alerta.Id}";
        await _emailService.EnviarEmailAsync(destinatario, asunto, mensaje);
    }

    private AlertaDTO MapToDTO(Alerta a)
    {
        return new AlertaDTO
        {
            Id = a.Id,
            ClienteId = a.ClienteId,
            TipoEvento = a.TipoEvento,
            MedioNotificacion = a.MedioNotificacion,
            DiasAntesVencimiento = a.DiasAntesVencimiento,
            EstadoFinancieroObjetivo = a.EstadoFinancieroObjetivo,
            Activa = a.Activa,
            EmailDestino = a.EmailDestino,
            FechaCreacion = a.FechaCreacion,
            FechaUltimaNotificacion = a.FechaUltimaNotificacion
        };
    }
} 