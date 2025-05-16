using System;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PortalAnalisisCrediticio.Core.Interfaces;

namespace PortalAnalisisCrediticio.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _configuration;
    private readonly SmtpClient _smtpClient;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private const int MAX_RETRY_ATTEMPTS = 3;
    private const int RETRY_DELAY_MS = 1000;

    public EmailService(
        ILogger<EmailService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _smtpClient = new SmtpClient
        {
            Host = _configuration["Email:SmtpHost"],
            Port = int.Parse(_configuration["Email:SmtpPort"]),
            EnableSsl = bool.Parse(_configuration["Email:EnableSsl"]),
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new System.Net.NetworkCredential(
                _configuration["Email:Username"],
                _configuration["Email:Password"]
            )
        };

        _fromEmail = _configuration["Email:FromEmail"];
        _fromName = _configuration["Email:FromName"];
    }

    public async Task EnviarEmailRecuperacionPasswordAsync(string email, string token)
    {
        try
        {
            _logger.LogInformation($"Iniciando envío de email de recuperación a: {email}");

            var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={token}";
            var subject = "Recuperación de Contraseña";
            var body = GeneratePasswordResetEmailBody(resetLink);

            await SendEmailWithRetryAsync(email, subject, body);
            _logger.LogInformation($"Email de recuperación enviado exitosamente a: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al enviar email de recuperación a: {email}");
            throw;
        }
    }

    public async Task EnviarEmailConfirmacionAsync(string email, string token)
    {
        try
        {
            _logger.LogInformation($"Iniciando envío de email de confirmación a: {email}");

            var confirmLink = $"{_configuration["AppUrl"]}/confirm-email?token={token}";
            var subject = "Confirmación de Email";
            var body = GenerateEmailConfirmationBody(confirmLink);

            await SendEmailWithRetryAsync(email, subject, body);
            _logger.LogInformation($"Email de confirmación enviado exitosamente a: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al enviar email de confirmación a: {email}");
            throw;
        }
    }

    public async Task EnviarEmailNotificacionAsync(string email, string subject, string message)
    {
        try
        {
            _logger.LogInformation($"Iniciando envío de email de notificación a: {email}");

            var body = GenerateNotificationEmailBody(message);
            await SendEmailWithRetryAsync(email, subject, body);
            _logger.LogInformation($"Email de notificación enviado exitosamente a: {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al enviar email de notificación a: {email}");
            throw;
        }
    }

    private async Task SendEmailWithRetryAsync(string to, string subject, string body)
    {
        for (int attempt = 1; attempt <= MAX_RETRY_ATTEMPTS; attempt++)
        {
            try
            {
                using (var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail, _fromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                })
                {
                    mailMessage.To.Add(to);
                    await _smtpClient.SendMailAsync(mailMessage);
                }
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Intento {attempt} de {MAX_RETRY_ATTEMPTS} fallido al enviar email a: {to}");
                
                if (attempt == MAX_RETRY_ATTEMPTS)
                {
                    throw;
                }

                await Task.Delay(RETRY_DELAY_MS * attempt);
            }
        }
    }

    private string GeneratePasswordResetEmailBody(string resetLink)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
        sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
        sb.AppendLine(".button { display: inline-block; padding: 10px 20px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; }");
        sb.AppendLine(".footer { margin-top: 20px; font-size: 12px; color: #666; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class='container'>");
        sb.AppendLine("<h2>Recuperación de Contraseña</h2>");
        sb.AppendLine("<p>Hemos recibido una solicitud para restablecer su contraseña.</p>");
        sb.AppendLine("<p>Haga clic en el siguiente botón para restablecer su contraseña:</p>");
        sb.AppendLine($"<p><a href='{resetLink}' class='button'>Restablecer Contraseña</a></p>");
        sb.AppendLine("<p>Si no solicitó este cambio, puede ignorar este mensaje.</p>");
        sb.AppendLine("<div class='footer'>");
        sb.AppendLine("<p>Este enlace expirará en 24 horas por razones de seguridad.</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerateEmailConfirmationBody(string confirmLink)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
        sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
        sb.AppendLine(".button { display: inline-block; padding: 10px 20px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; }");
        sb.AppendLine(".footer { margin-top: 20px; font-size: 12px; color: #666; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class='container'>");
        sb.AppendLine("<h2>Confirmación de Email</h2>");
        sb.AppendLine("<p>Gracias por registrarse. Por favor confirme su dirección de email.</p>");
        sb.AppendLine("<p>Haga clic en el siguiente botón para confirmar su email:</p>");
        sb.AppendLine($"<p><a href='{confirmLink}' class='button'>Confirmar Email</a></p>");
        sb.AppendLine("<div class='footer'>");
        sb.AppendLine("<p>Este enlace expirará en 24 horas.</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }

    private string GenerateNotificationEmailBody(string message)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<style>");
        sb.AppendLine("body { font-family: Arial, sans-serif; line-height: 1.6; color: #333; }");
        sb.AppendLine(".container { max-width: 600px; margin: 0 auto; padding: 20px; }");
        sb.AppendLine(".message { background-color: #f8f9fa; padding: 20px; border-radius: 5px; }");
        sb.AppendLine(".footer { margin-top: 20px; font-size: 12px; color: #666; }");
        sb.AppendLine("</style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<div class='container'>");
        sb.AppendLine("<h2>Notificación</h2>");
        sb.AppendLine("<div class='message'>");
        sb.AppendLine($"<p>{message}</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("<div class='footer'>");
        sb.AppendLine("<p>Este es un mensaje automático, por favor no responda a este email.</p>");
        sb.AppendLine("</div>");
        sb.AppendLine("</div>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        return sb.ToString();
    }
} 