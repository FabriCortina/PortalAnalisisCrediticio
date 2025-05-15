namespace PortalAnalisisCrediticio.Core.Entities;

public class ClienteEmpresa
{
    public int ClienteId { get; set; }
    public int EmpresaId { get; set; }
    public string Rol { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public bool Activo { get; set; }

    // Relaciones
    public Cliente Cliente { get; set; }
    public Empresa Empresa { get; set; }
} 