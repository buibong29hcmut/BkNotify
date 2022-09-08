using BkMail.Contants;
using BkMail.Contracts;
using Newtonsoft.Json.Linq;

namespace BkMail.Services
{
    public class CheckInfoApi:ICheckInfoApi
    {
        private readonly IHttpClientFactory _httpClientFactory; 
        public CheckInfoApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<bool> Check(string wtToken)
        {
            Dictionary<string, string> param = new Dictionary<string, string>()
            {
                {"wstoken",wtToken },
                {"moodlewsrestformat","json"},
                {"wsfunction", WsFunctionConst.GetUserId },

            };
            using (var httpClient = _httpClientFactory.CreateClient("moodleAPI"))
            {
                var encodedContent = new FormUrlEncodedContent(param);
                var queryString = await encodedContent.ReadAsStringAsync();
                var result = await httpClient.GetStringAsync(httpClient.BaseAddress + $"?{queryString}");

                var data = JObject.Parse(result);
                try
                {
                    var userGuid = Convert.ToInt32(Convert.ToString(data["preferences"]["userid"]));
                    if (userGuid!=null)
                        return true;
                }
                catch
                {
                    return false;
                }
                return false;
            }

        }

    }
}
