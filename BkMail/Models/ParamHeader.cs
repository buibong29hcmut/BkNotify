namespace BkMail.Models
{
    public class ParamHeader
    {
        public string WsToken { get; set; }
        public string moodlewsrestformat { get; private set; } = "json";
        public Dictionary<string, string> OtherParam { get; set; } = new Dictionary<string, string>();
    }
}
