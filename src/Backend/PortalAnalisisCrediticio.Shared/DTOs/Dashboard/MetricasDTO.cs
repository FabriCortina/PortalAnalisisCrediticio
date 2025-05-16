using System;

namespace PortalAnalisisCrediticio.Shared.DTOs.Dashboard
{
    public class MetricasDTO
    {
        public int TotalClientes { get; set; }
        public int TotalCreditos { get; set; }
        public decimal MontoTotalPrestado { get; set; }
        public int CreditosActivos { get; set; }
        public int CreditosVencidos { get; set; }
        public decimal MontoVencido { get; set; }
        public decimal TasaVencimiento { get; set; }
    }
} 