using BkMail.Models;

namespace BkMail.Contracts
{
    public interface IEventBkApi
    {
        Task<List<EventModel>> GetAllEventsBeforeExpire2Days(string WsToken);
        Task<List<EventModel>> GetAllEventsBeforeExpires(string WsToken, double numberDay);



    }
}
