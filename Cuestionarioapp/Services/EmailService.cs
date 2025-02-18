using Cuestionarioapp.Models;
using System.Net.Mail;
using System.Net;

namespace Cuestionarioapp.Services
{
    public class EmailService :IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendResponseEmailAsync(string to, UserResponse response)
        {
            try
            {
                using var client = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
                {
                    Port = int.Parse(_configuration["EmailSettings:Port"]),
                    Credentials = new NetworkCredential(
                        _configuration["EmailSettings:Username"],
                        _configuration["EmailSettings:Password"]
                    ),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(_configuration["EmailSettings:FromEmail"]),
                    Subject = "Quiz Response",
                    Body = $@"
                    <h2>Pregunta:</h2>
                    <p>{response.Question.Text}</p>
                    <h2>Tu Respuesta:</h2>
                    <p>{response.Response}</p>
                    <p><small>Enviada el: {response.CreatedAt:g}</small></p>
                ",
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email");
                throw;
            }
        }
    }
}
