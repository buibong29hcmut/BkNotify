using BkMail.Models;

namespace BkMail.Services
{
    public interface IMessageBkApi
    {
        Task<MessagesList> GetAllMessageByUserId(int userId, string Token,StatusMessage status);
        Task<MessagesList> GetAllMessageUnreadByUserId(int userId, string Token);
        Task<MessagesList> GetAllMessageUnreadByToken(string Token);

    }
}
