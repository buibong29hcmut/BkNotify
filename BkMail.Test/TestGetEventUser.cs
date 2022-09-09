using BkMail.Contracts;
using BkMail.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BkMail.Test
{
    [TestClass]   
    public class TestGetEventUser
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            ServiceContainer container = new ServiceContainer();
            var services = container.CreateService();
            string token = "975b9798c0182700883d878f36cc9e19";
            var eventApi = services.BuildServiceProvider().GetRequiredService<IEventBkApi>();
            var result= await eventApi.GetAllEventsBeforeExpires(token, 14);
            Assert.AreEqual(result.Count() > 0, true);
           
        }
    }
}
