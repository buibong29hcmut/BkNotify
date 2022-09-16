using BkMail.Context;
using BkMail.Entities;
using BkMail.Helpers;
using BkMail.Services;
using BkMail;
using Microsoft.EntityFrameworkCore;
using System;
using BkMail.Models;
using BkMail.Contracts;

public class BkWorkerSendEventMessage:BackgroundService
{
    private readonly ILogger<BkWorkerSendEventMessage> _logger;
    private readonly IServiceProvider _serviceProvider;
    public BkWorkerSendEventMessage(ILogger<BkWorkerSendEventMessage> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await DoSendEventUser();

            await Task.Delay(10000, stoppingToken);
        }

    }
    private async Task DoSendEventUser()
    {
     
        using (var scope = _serviceProvider.CreateScope())
        {
            var _dbFactory = scope.ServiceProvider.GetService<IDbContextFactory<BkDbContext>>();
            BkMail.Services.IMailService _mailService = scope.ServiceProvider.GetService<BkMail.Services.IMailService>();
            
            IEventBkApi _eventApi = scope.ServiceProvider.GetService<IEventBkApi>();
            using(var db= _dbFactory.CreateDbContext())
            {
                var allUser =await db.StudentDatas.ToListAsync();
                foreach(var user in allUser)
                {
                    try
                    {
                        var messageEvent = await _eventApi.GetAllEventsBeforeExpires(user.WsToken,2);
                        foreach (var eventItem in messageEvent)
                        {
                            if (db.EventDatas.Any(p => p.WsToken == user.WsToken && p.EventId == eventItem.Id))
                            {
                                continue;
                            }
                            MailMessage mail = new MailMessage()
                            {
                                Subject = eventItem.Name,
                                BodyHtml ="<br>"+ eventItem.Name + "/<br>" +"<br>"+ eventItem.Descrption + "</br>" +"<br>"+ eventItem.EndDay + "</br>" +"<br>"+"Link bài tập"+eventItem.LinkAssignMent+"</br>",
                                ToUser = user.Email,


                            };
                            await _mailService.SendMailAsync(mail);
                            await db.EventDatas.AddAsync(new EventData()
                            {
                                WsToken = user.WsToken,
                                EventId = eventItem.Id,
                            });
                            await db.SaveChangesAsync();

                        }
                    }
                    catch(Exception ex)
                    {
                        _logger.LogWarning(ex.Message);
                        continue;
                    }
                }
            }
 
        }
    }
}
