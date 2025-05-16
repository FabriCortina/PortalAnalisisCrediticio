using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class ClienteRiesgoDTO
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string NivelRiesgo { get; set; }
        public decimal MontoTotalCredito { get; set; }
        public int CreditosActivos { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
} 