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
<table width="100%" cellpadding="0" cellspacing="0" style="font-family: Arial, sans-serif; background-color: #f9f9f9; padding: 20px;">
    <tr>
        <td align="center">
            <table width="600" cellpadding="0" cellspacing="0" style="background-color: #ffffff; border-radius: 10px; overflow: hidden;">
                <tr>
                    <td style="background-color: #1e90ff; padding: 20px; text-align: center; color: white;">
                        <h2 style="margin: 0;">EVENTOP</h2>
                        <p style="margin: 0;">Tài khoản nhà cung cấp đã được phê duyệt</p>
                    </td>
                </tr>
                <tr>
                    <td style="padding: 30px;">
                        <p>Xin chào <strong>{toEmail}</strong>,</p>
                        <p>Tài khoản nhà cung cấp của bạn đã được <strong>phê duyệt</strong>.</p>
                        <p>Dưới đây là thông tin đăng nhập của bạn:</p>
                        <table cellpadding="5" cellspacing="0" style="background-color: #f1f1f1; border-radius: 5px; padding: 10px;">
                            <tr>
                                <td><strong>Email:</strong></td>
                                <td>{toEmail}</td>
                            </tr>
                            <tr>
                                <td><strong>Mật khẩu:</strong></td>
                                <td>{plainPassword}</td>
                            </tr>
                        </table>
                        <p style="margin-top: 20px;">Vui lòng đăng nhập và <strong>đổi mật khẩu ngay</strong> sau khi đăng nhập lần đầu để đảm bảo bảo mật.</p>
                        <p style="text-align: center; margin: 30px 0;">
                            <a href="https://eventop.vercel.app/" style="background-color: #1e90ff; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;">
                                Đăng nhập ngay
                            </a>
                        </p>
                        <p>Nếu bạn không yêu cầu tạo tài khoản này, vui lòng liên hệ với đội ngũ hỗ trợ của chúng tôi.</p>
                        <p>Trân trọng,<br>Đội ngũ EVENTOP</p>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #f1f1f1; text-align: center; font-size: 12px; color: #777; padding: 10px;">
                        © 2025 EVENTOP. All rights reserved.
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
"""
,

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
                EmailType.ProvideAccountSupplier => "Tài khoản nhà cung cấp được phê duyệt",
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
