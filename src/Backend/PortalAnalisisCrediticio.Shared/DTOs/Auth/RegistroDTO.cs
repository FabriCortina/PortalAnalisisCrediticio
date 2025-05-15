using System.ComponentModel.DataAnnotations;

namespace PortalAnalisisCrediticio.Shared.DTOs.Auth;

public class RegistroDTO
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(100)]
    public string Apellido { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    [Compare("Password")]
    public string ConfirmarPassword { get; set; }

    [Required]
    public bool AceptoTerminos { get; set; }

    public bool AceptoMarketing { get; set; }
} 