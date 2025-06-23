using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharedLibrary.Email.EmailService;

namespace SharedLibrary.Email
{
        public class EmailService 
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
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "Templates", "ConfirmEmail.html");


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
                message.Subject = "Xác nhận email đăng ký tài khoản EVENTOP";

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
