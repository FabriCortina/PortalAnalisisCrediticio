using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PortalAnalisisCrediticio.Shared.DTOs.Auditoria;

namespace PortalAnalisisCrediticio.Core.Interfaces
{
    public interface IAuditoriaService
    {
        Task RegistrarActividadAsync(string usuarioId, string accion, string detalle, string ipAddress);
        Task<List<ActividadDTO>> ObtenerActividadesAsync(DateTime fechaInicio, DateTime fechaFin, string usuarioId = null);
        Task<List<ActividadDTO>> ObtenerActividadesPorTipoAsync(string tipoActividad, DateTime fechaInicio, DateTime fechaFin);
        Task<EstadisticasAuditoriaDTO> ObtenerEstadisticasAsync(DateTime fechaInicio, DateTime fechaFin);
    }
} 