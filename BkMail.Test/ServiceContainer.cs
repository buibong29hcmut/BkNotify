
using BkMail.Contracts;
using BkMail.Models;
using BkMail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace BkMail.Test
{
    public class ServiceContainer
    {
       public IServiceCollection CreateService()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                 .AddEnvironmentVariables()
                 .Build();
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
           services.AddHttpClient("moodleAPI", p =>
            {
                p.BaseAddress = new Uri("http://e-learning.hcmut.edu.vn/webservice/rest/server.php");
            });
            services.AddSingleton(config);
            services.AddOptions();
            services.Configure<MailSettings>(config.GetSection("MailSettings"));
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IMessageBkApi, MessageBkApi>();
            services.AddScoped<IEventBkApi, EventBkApi>();

            return services;
        }
    }
}
