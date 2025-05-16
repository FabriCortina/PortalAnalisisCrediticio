using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Producto;

public class UpdateProductoDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
    public string Nombre { get; set; }

    [Required(ErrorMessage = "El código interno es requerido")]
    [MaxLength(50, ErrorMessage = "El código interno no puede exceder los 50 caracteres")]
    public string CodigoInterno { get; set; }

    [Required(ErrorMessage = "El precio unitario es requerido")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "La moneda es requerida")]
    [MaxLength(3, ErrorMessage = "La moneda debe tener 3 caracteres")]
    public string Moneda { get; set; }

    public bool Activo { get; set; }
} 