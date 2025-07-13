using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using SharedLibrary.Enum;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SharedLibrary.Email
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string? token, EmailType emailType, string? plainPassword = null)
        {
            var emailSettings = GetEmailSettings();

            string emailBody = emailType switch
            {
                EmailType.Register => LoadEmailBodyTemplate(toEmail, GenerateConfirmLink(token)),

                EmailType.SupplierRequest => $"""
            <p>Chào {toEmail},</p>
            <p>Yêu cầu đăng ký trở thành nhà cung cấp của bạn đã được tiếp nhận.</p>
            <p>Chúng tôi sẽ liên hệ lại sau khi kiểm duyệt thông tin.</p>
            """,

                EmailType.ForgotPassword => $"""
            <p>Chào {toEmail},</p>
            <p>Bạn đã yêu cầu đặt lại mật khẩu. Vui lòng nhấn vào liên kết sau:</p>
            <a href="{GenerateConfirmLink(token)}">{GenerateConfirmLink(token)}</a>
            """,

                EmailType.ApprovalNotice => $"""
            <p>Chào {toEmail},</p>
            <p>Tài khoản của bạn đã được duyệt. Bạn có thể đăng nhập và sử dụng hệ thống.</p>
            """,

                EmailType.ProvideAccountSupplier => $"""
            <p>Chào {toEmail},</p>
            <p>Tài khoản nhà cung cấp của bạn đã được duyệt.</p>
            <p>Thông tin đăng nhập:</p>
            <ul>
                <li><strong>Email:</strong> {toEmail}</li>
                <li><strong>Mật khẩu:</strong> {plainPassword}</li>
            </ul>
            <p>Vui lòng đăng nhập và đổi mật khẩu ngay sau khi sử dụng lần đầu.</p>
            """,

                _ => "<p>Xin chào, chúng tôi đã nhận được yêu cầu từ bạn.</p>"
            };

            var emailMessage = BuildEmailMessage(toEmail, emailBody, emailSettings, emailType);
            await SendEmailViaSmtp(emailMessage, emailSettings);
        }


        private IConfigurationSection GetEmailSettings()
        {
            return _configuration.GetSection("EMAIL");
        }

        private string GenerateConfirmLink(string token)
        {
            var baseUrl = _configuration["APP_FRONTEND_BASE_URL"] ?? "https://your-default-domain.com";
            return $"{baseUrl}/confirm?token={Uri.EscapeDataString(token)}";
        }

        private string LoadEmailBodyTemplate(string email, string confirmLink)
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Email", "Templates", "ConfirmEmail.html");

            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Email template not found", templatePath);

            var htmlTemplate = File.ReadAllText(templatePath);

            return htmlTemplate
                .Replace("{{username}}", email)
                .Replace("{{confirm_link}}", confirmLink);
        }

        private MimeMessage BuildEmailMessage(string toEmail, string body, IConfigurationSection emailSettings, EmailType emailType)
        {
            var subject = emailType switch
            {
                EmailType.Register => "Xác nhận email đăng ký tài khoản EVENTOP",
                EmailType.SupplierRequest => "Yêu cầu trở thành nhà cung cấp",
                EmailType.ForgotPassword => "Khôi phục mật khẩu EVENTOP",
                EmailType.ApprovalNotice => "Tài khoản của bạn đã được duyệt",
                _ => "Thông báo từ EVENTOP"
            };

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(emailSettings["SENDER_NAME"], emailSettings["SENDER_EMAIL"]));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }

        private async Task SendEmailViaSmtp(MimeMessage message, IConfigurationSection emailSettings)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["SMTP_SERVER"], int.Parse(emailSettings["SMTP_PORT"]), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(emailSettings["SENDER_EMAIL"], emailSettings["SENDER_PASSWORD"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
