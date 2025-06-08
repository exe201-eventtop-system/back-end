using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string token)
        {
            var emailSettings = GetEmailSettings();
            var confirmLink = GenerateConfirmLink(token);
            var emailBody = LoadEmailBodyTemplate(toEmail, confirmLink);
            var emailMessage = BuildEmailMessage(toEmail, emailBody, emailSettings);

            await SendEmailViaSmtp(emailMessage, emailSettings);
        }
        private IConfigurationSection GetEmailSettings()
        {
            return _configuration.GetSection("Email");
        }
        private string GenerateConfirmLink(string token)
        {
            return $"{_configuration["App:FrontendBaseUrl"]}/confirm?token={Uri.EscapeDataString(token)}";
        }

        private string LoadEmailBodyTemplate(string email, string confirmLink)
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\"));
            var templatePath = Path.Combine(projectRoot, @"Application\Helper\Templates\ConfirmEmail.html");

            var htmlTemplate = File.ReadAllText(templatePath);

            return htmlTemplate
                .Replace("{{username}}", email)
                .Replace("{{confirm_link}}", confirmLink);
        }

        private MimeMessage BuildEmailMessage(string toEmail, string body, IConfigurationSection emailSettings)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SenderName"], emailSettings["SenderEmail"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Xác nhận email đăng ký";

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }
        private async Task SendEmailViaSmtp(MimeMessage message, IConfigurationSection emailSettings)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SmtpServer"], int.Parse(emailSettings["SmtpPort"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["SenderEmail"], emailSettings["SenderPassword"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
