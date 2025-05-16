using System;

namespace PortalAnalisisCrediticio.Core.Entities
{
    public class SolicitudProducto
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Producto { get; set; }
        public decimal MontoSolicitado { get; set; }
        public decimal SaldoActual { get; set; }
        public decimal TasaInteres { get; set; }
        public DateTime FechaOtorgamiento { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public string Estado { get; set; }
        public Cliente Cliente { get; set; }
    }
} 