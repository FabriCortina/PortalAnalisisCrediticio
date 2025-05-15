namespace PortalAnalisisCrediticio.Core.Entities;

public class Empresa
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
    public string TipoEmpresa { get; set; }
    public string Sector { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
    public bool Activa { get; set; }

    // Relaciones
    public ICollection<ClienteEmpresa> ClienteEmpresas { get; set; }
} 