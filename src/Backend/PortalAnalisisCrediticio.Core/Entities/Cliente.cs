namespace PortalAnalisisCrediticio.Core.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string CUIT { get; set; }
    public string Email { get; set; }
    public string Telefono { get; set; }
    public string Direccion { get; set; }
    public string Ciudad { get; set; }
    public string Provincia { get; set; }
    public string Pais { get; set; }
    public string CodigoPostal { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string EstadoCivil { get; set; }
    public string Ocupacion { get; set; }
    public string Nacionalidad { get; set; }
    public string TipoDocumento { get; set; }
    public string NumeroDocumento { get; set; }
    public DateTime FechaAlta { get; set; }
    public DateTime? FechaBaja { get; set; }
    public bool Activo { get; set; }

    // Relaciones
    public InformacionFinanciera InformacionFinanciera { get; set; }
    public ICollection<ClienteEmpresa> ClienteEmpresas { get; set; }
} 