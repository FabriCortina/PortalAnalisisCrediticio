using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class CreditoVencidoDTO
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public int DiasVencido { get; set; }
        public string Estado { get; set; }
    }
} 