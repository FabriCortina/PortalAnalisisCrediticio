namespace PortalAnalisisCrediticio.Shared.DTOs;

public class ClienteEmpresaDTO
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int EmpresaId { get; set; }
    public DateTime FechaAsociacion { get; set; }
    public string Estado { get; set; }
    public string Observaciones { get; set; }

    // Relaciones
    public ClienteDTO Cliente { get; set; }
    public EmpresaDTO Empresa { get; set; }
} 