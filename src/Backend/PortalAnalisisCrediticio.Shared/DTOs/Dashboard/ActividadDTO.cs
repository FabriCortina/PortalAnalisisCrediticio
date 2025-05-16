using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class ActividadDTO
    {
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Accion { get; set; }
        public string Entidad { get; set; }
        public string Detalles { get; set; }
        public DateTime Fecha { get; set; }
    }
} 