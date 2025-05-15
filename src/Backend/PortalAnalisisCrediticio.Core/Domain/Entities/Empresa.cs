using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Core.Domain.Entities;

public class Empresa
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(11)]
    public string CUIT { get; set; }

    [MaxLength(200)]
    public string Direccion { get; set; }

    [MaxLength(20)]
    public string Telefono { get; set; }

    [MaxLength(100)]
    public string Email { get; set; }

    // Relaciones
    public ICollection<ClienteEmpresa> ClienteEmpresas { get; set; }
} 