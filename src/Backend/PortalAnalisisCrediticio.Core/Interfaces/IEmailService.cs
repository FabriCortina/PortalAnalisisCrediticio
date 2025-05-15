namespace PortalAnalisisCrediticio.Core.Interfaces;

public interface IEmailService
{
    Task EnviarEmailRecuperacionPasswordAsync(string email, string token);
    Task EnviarEmailConfirmacionAsync(string email, string token);
    Task EnviarEmailNotificacionAsync(string email, string asunto, string mensaje);
} 