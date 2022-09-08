using BkMail.Contants;
using BkMail.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BkMail.Services
{
    public class MessageBkApi : IMessageBkApi
    {
        private readonly IHttpClientFactory _factory;
        public MessageBkApi(IHttpClientFactory factory)
        {
            _factory = factory;
        }
        public async Task<MessagesList> GetAllMessageByUserId(int userId, string Token, StatusMessage status)
        {
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"wstoken",Token },
                {"moodlewsrestformat","json"},
                {"wsfunction", WsFunctionConst.GetMessageUser },
                {"useridto",userId.ToString() },
                {"read", ((int)status).ToString()},
            };
            using (var httpClient = _factory.CreateClient("moodleAPI"))
            {
                var encodedContent = new FormUrlEncodedContent(param);
                var queryString = await encodedContent.ReadAsStringAsync();
                var result= await httpClient.GetFromJsonAsync<MessagesList>(httpClient.BaseAddress +$"?{queryString}");
                return result;
            }
        }
        public async Task<MessagesList> GetAllMessageUnreadByToken(string Token)
        {
            int UserId = await GetUserId(Token);
            return await GetAllMessageUnreadByUserId(UserId, Token);
        }
        public async Task<MessagesList> GetAllMessageUnreadByUserId(int userId, string Token)
        {
            return await GetAllMessageByUserId(userId, Token, StatusMessage.UnRead);
        }
        private async Task<int> GetUserId(string Token)
        {
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"wstoken",Token },
                {"moodlewsrestformat","json"},
                {"wsfunction", WsFunctionConst.GetUserId },
             
            };
            using (var httpClient = _factory.CreateClient("moodleAPI"))
            {
                var encodedContent = new FormUrlEncodedContent(param);
                var queryString = await encodedContent.ReadAsStringAsync();
                var result = await httpClient.GetStringAsync(httpClient.BaseAddress + $"?{queryString}");

                var data = JObject.Parse(result);
                var userGuid = Convert.ToString(data["preferences"]["userid"]);
                return Convert.ToInt32(userGuid);
            }
            
        }
    }
}
