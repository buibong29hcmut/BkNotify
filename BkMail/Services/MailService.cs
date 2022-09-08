using BkMail.Models;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BkMail.Services
{
    public class MailService:IMailService
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<MailService> _logger;
        public MailService(IOptions<MailSettings> _mailSettings, ILogger<MailService> logger)
        {
            mailSettings = _mailSettings.Value;
            _logger = logger;
            _logger.LogInformation("Create SendMailService");
        }
        public async Task SendMailAsync(MailMessage mail)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            email.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mail.ToUser));
            email.Subject = mail.Subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = mail.BodyHtml;
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

          
                smtp.AuthenticationMechanisms.Remove("XOAUTH2");

                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(email);
            
        
            smtp.Disconnect(true);

            _logger.LogInformation("send mail to " + mail.ToUser);

        }
    }
}
