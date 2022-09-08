namespace BkMail.Entities
{
    public class StudentData:Entity
    {
        public string WsToken { get; private set; }
        public string Email { get; private set; }
        public StudentData(string wsToken, string email)
        {
            WsToken = wsToken;
            Email = email;
        }

    }
}
