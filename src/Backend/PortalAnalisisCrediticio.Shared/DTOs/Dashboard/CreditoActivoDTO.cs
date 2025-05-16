using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class CreditoActivoDTO
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public string TipoCredito { get; set; }
    }
} 