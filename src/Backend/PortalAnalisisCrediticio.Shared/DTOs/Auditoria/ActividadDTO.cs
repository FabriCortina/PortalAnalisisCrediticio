using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Auditoria
{
    public class ActividadDTO
    {
        public string Id { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioNombre { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public string IpAddress { get; set; }
        public DateTime Fecha { get; set; }
        public string TipoActividad { get; set; }
    }

    public class EstadisticasAuditoriaDTO
    {
        public int TotalActividades { get; set; }
        public int ActividadesPorUsuario { get; set; }
        public Dictionary<string, int> ActividadesPorTipo { get; set; }
        public Dictionary<string, int> ActividadesPorHora { get; set; }
        public List<string> IpsUnicas { get; set; }
    }
} 