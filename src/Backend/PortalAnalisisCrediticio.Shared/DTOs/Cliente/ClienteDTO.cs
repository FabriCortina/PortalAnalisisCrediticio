using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PortalAnalisisCrediticio.Shared.DTOs.Empresa;
using PortalAnalisisCrediticio.Shared.DTOs.InformacionFinanciera;

namespace PortalAnalisisCrediticio.Shared.DTOs.Cliente;

public class ClienteDTO
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; }
    
    [Required]
    [MaxLength(11)]
    public string CUIT { get; set; }
    
    [MaxLength(100)]
    public string Email { get; set; }
    
    [MaxLength(20)]
    public string Telefono { get; set; }
    
    // Relaciones
    public InformacionFinancieraDTO InformacionFinanciera { get; set; }
    public List<ClienteEmpresaDTO> ClienteEmpresas { get; set; }
} 