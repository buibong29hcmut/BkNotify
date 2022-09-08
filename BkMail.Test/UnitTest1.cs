using BkMail.Models;
using BkMail.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BkMail.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            ServiceContainer container = new ServiceContainer();
            var services = container.CreateService();
            string token = "5822ded814e08fa5a448bed3e0099f57";
            var mailService = services.BuildServiceProvider().GetRequiredService<IMailService>();
            var messageApi = services.BuildServiceProvider().GetRequiredService<IMessageBkApi>();
            var result= await messageApi.GetAllMessageUnreadByToken(token);
            for(int i = 0; i < result.messages.Count; i++)
            {
                var mail = new MailMessage()
                {
                    BodyHtml = result.messages[i].fullmessagehtml,
                    ToUser = "bong.buibuibong29@hcmut.edu.vn",
                    Subject = result.messages[i].subject
                };
                await mailService.SendMailAsync(mail);
            }
            Assert.AreEqual(result.messages.Count > 0, 0);
        }
    }
}