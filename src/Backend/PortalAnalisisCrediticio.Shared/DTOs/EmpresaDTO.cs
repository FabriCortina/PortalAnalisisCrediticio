namespace PortalAnalisisCrediticio.Shared.DTOs;

public class EmpresaDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string CUIT { get; set; }
    public string Direccion { get; set; }
    public string Ciudad { get; set; }
    public string Provincia { get; set; }
    public string Pais { get; set; }
    public string CodigoPostal { get; set; }
    public string Telefono { get; set; }
    public string Email { get; set; }
    public string SitioWeb { get; set; }
    public string MonedaPredeterminada { get; set; }
    public string IdiomaPredeterminado { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
    public bool Activo { get; set; }

    // Relaciones
    public ICollection<ClienteEmpresaDTO> ClienteEmpresas { get; set; }
} 