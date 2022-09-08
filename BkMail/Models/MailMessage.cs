namespace BkMail.Models
{
    public class MailMessage
    {
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public string UserFrom { get; set; }
        public string ToUser { get; set; }
    }
}
