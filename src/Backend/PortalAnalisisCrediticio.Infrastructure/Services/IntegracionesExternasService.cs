using Microsoft.EntityFrameworkCore;
using PortalAnalisisCrediticio.Core.Entities;
using PortalAnalisisCrediticio.Core.Interfaces;
using PortalAnalisisCrediticio.Infrastructure.Data;
using PortalAnalisisCrediticio.Shared.DTOs.Integraciones;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class IntegracionesExternasService : IIntegracionesExternasService
{
    private readonly ApplicationDbContext _context;

    public IntegracionesExternasService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<InformeNosisDTO> GetInformeNosisAsync(int clienteId)
    {
        var informe = new InformeNosisDTO();
        await GuardarInformeAsync(clienteId, "Nosis", informe);
        return informe;
    }

    public async Task<InformeVerazDTO> GetInformeVerazAsync(int clienteId)
    {
        var informe = new InformeVerazDTO();
        await GuardarInformeAsync(clienteId, "Veraz", informe);
        return informe;
    }

    public async Task<InformeInfoexpertoDTO> GetInformeInfoexpertoAsync(int clienteId)
    {
        var informe = new InformeInfoexpertoDTO();
        await GuardarInformeAsync(clienteId, "Infoexperto", informe);
        return informe;
    }

    public async Task<InformeBCRADTO> GetInformeBCRAAsync(int clienteId)
    {
        var informe = new InformeBCRADTO();
        await GuardarInformeAsync(clienteId, "BCRA", informe);
        return informe;
    }

    public async Task<InformeAFIPDTO> GetInformeAFIPAsync(int clienteId)
    {
        var informe = new InformeAFIPDTO();
        await GuardarInformeAsync(clienteId, "AFIP", informe);
        return informe;
    }

    public async Task<InformePublicoDTO> GetInformePublicoAsync(int clienteId)
    {
        var informe = new InformePublicoDTO();
        await GuardarInformeAsync(clienteId, "Publico", informe);
        return informe;
    }

    private async Task GuardarInformeAsync<T>(int clienteId, string fuente, T informe) where T : class
    {
        var informeExterno = new InformeExterno
        {
            ClienteId = clienteId,
            Fuente = fuente,
            Estado = (string)informe.GetType().GetProperty("Estado").GetValue(informe),
            Score = (string)informe.GetType().GetProperty("Score").GetValue(informe),
            Riesgo = (string)informe.GetType().GetProperty("Riesgo").GetValue(informe),
            UltimaActualizacion = (string)informe.GetType().GetProperty("UltimaActualizacion").GetValue(informe),
            Observaciones = (string)informe.GetType().GetProperty("Observaciones").GetValue(informe)
        };

        _context.InformesExternos.Add(informeExterno);
        await _context.SaveChangesAsync();
    }
} 