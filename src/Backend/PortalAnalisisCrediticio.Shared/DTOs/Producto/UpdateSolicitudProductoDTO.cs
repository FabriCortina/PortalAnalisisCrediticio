using System;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Producto
{
    public class UpdateSolicitudProductoDTO
    {
        [Required]
        [MaxLength(50)]
        public string TipoProducto { get; set; }
        
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal MontoSolicitado { get; set; }
        
        [Required]
        [MaxLength(3)]
        public string Moneda { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int PlazoMeses { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Estado { get; set; }
        
        public DateTime? FechaAprobacion { get; set; }
        
        public DateTime? FechaRechazo { get; set; }
        
        [MaxLength(500)]
        public string MotivoRechazo { get; set; }
        
        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; }
        
        [MaxLength(500)]
        public string Observaciones { get; set; }
    }
} 