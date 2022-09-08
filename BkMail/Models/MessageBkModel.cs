namespace BkMail.Models
{
    public class MessageBkModel
    {   public int id { get; set; }
        public string userfromfullname { get; set; }
        public string subject { get; set; }
        public string fullmessagehtml { get; set; }
        public double timecreated { get; set; }
    }
}
