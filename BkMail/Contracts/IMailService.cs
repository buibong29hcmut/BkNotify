
using BkMail.Models;

namespace BkMail.Services
{
    public interface IMailService
    {
        Task SendMailAsync(MailMessage mail);
    }
}
