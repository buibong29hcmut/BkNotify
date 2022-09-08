namespace BkMail.Contracts
{
    public interface ICheckInfoApi
    {
        public  Task<bool> Check(string wtToken);
    }
}
