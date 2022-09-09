using BkMail.Context;
using BkMail.Entities;
using BkMail.Helpers;
using BkMail.Models;
using BkMail.Services;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Security;

namespace BkMail
{
    public class BkWorkerSendMessage: BackgroundService
    {
        private readonly ILogger<BkWorkerSendMessage> _logger;
        private readonly IServiceProvider _serviceProvider;
        public BkWorkerSendMessage(ILogger<BkWorkerSendMessage> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoSendMessageUser();
                    
                await Task.Delay(300, stoppingToken);
            }
            
        }
        private async Task DoSendMessageUser()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _dbFactory = scope.ServiceProvider.GetService<IDbContextFactory<BkDbContext>>();
                BkMail.Services.IMailService _mailService = scope.ServiceProvider.GetService<BkMail.Services.IMailService>();
                IMessageBkApi _messageBkApi = scope.ServiceProvider.GetService<IMessageBkApi>();
                using (var db = _dbFactory.CreateDbContext())
                {
                    var allUser = db.StudentDatas.ToList();
                    foreach (var user in allUser)
                    {
                        try
                        {
                            var listMessageUnread = await _messageBkApi.GetAllMessageUnreadByToken(user.WsToken);
                            foreach (var message in listMessageUnread.messages)
                            {
                                if (db.MailSents.Any(p => p.MailId == message.id))
                                    continue;
                                var mail = new MailMessage()
                                {
                                    BodyHtml = message.fullmessagehtml,
                                    ToUser = user.Email,
                                    Subject = "New message từ" + " " + message.userfromfullname + "-" + message.subject + "-" + "Lúc " + ConvertDateTimeUnixToNormalDatetime.UnixTimeStampToDateTime(message.timecreated).ToString(),
                                };
                                await _mailService.SendMailAsync(mail);
                                await db.AddAsync(new MailSent()
                                {
                                    MailId = message.id,
                                });

                                await db.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            continue;
                        }
                    }
                }
            }
        }
    }
}
