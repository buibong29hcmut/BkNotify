using BkMail.Contants;
using BkMail.Contracts;
using BkMail.Models;
using Newtonsoft.Json.Linq;

namespace BkMail.Services
{
    public class EventBkApi:IEventBkApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public EventBkApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<EventModel>> GetAllEventsBeforeExpire2Days(string WsToken)
        {
            return await GetAllEventsBeforeExpires(WsToken, 2);
        }
        public async Task<List<EventModel>> GetAllEventsBeforeExpires(string WsToken,double numberDay)
        {
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"wstoken",WsToken },
                {"moodlewsrestformat","json"},
                {"wsfunction", WsFunctionConst.GetEventByTimeSort },

            };
            using (var httpClient = _httpClientFactory.CreateClient("moodleAPI"))
            {
                var encodedContent = new FormUrlEncodedContent(param);
                var queryString = await encodedContent.ReadAsStringAsync();
                var reponse = await httpClient.GetStringAsync(httpClient.BaseAddress + $"?{queryString}");
                var data = JObject.Parse(reponse);
                var allEvents = JArray.Parse(data["events"].ToString());
                List<EventModel> result = new List<EventModel>();
                foreach (var eventItem in allEvents)
                {
                    long unixFinalAssignment = eventItem.Value<long>("timestart");
                    double time = unixFinalAssignment - DateTimeOffset.Now.ToUnixTimeSeconds();
                    if (time >86400 * numberDay|| time<0)
                    {
                        continue;
                    }
                    EventModel model = new EventModel();
                    model.Id = eventItem.Value<int>("id");
                    model.Descrption = eventItem.Value<string>("description");
                    model.Name = eventItem.Value<string>("name");
                    model.ActivityName = eventItem.Value<string>("activityname");
                    model.LinkAssignMent = eventItem.Value<string>("url");
                    model.CourseName = eventItem["course"].Value<string>("fullname");
                    model.EndDay ="Bài kiểm tra kết thúc vào lúc "+ eventItem.Value<string>("formattedtime");
;                   result.Add(model);

                }
                return result;
            }
        }
    }
}
