using BkMail;
using BkMail.Context;
using BkMail.Contracts;
using BkMail.Data;
using BkMail.Models;
using BkMail.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddHttpClient("moodleAPI",p =>
{
    p.BaseAddress = new Uri("http://e-learning.hcmut.edu.vn/webservice/rest/server.php");
});
builder.Services.AddDbContextFactory<BkDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("BkConnectionString")));
builder.Services.AddOptions();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IMessageBkApi, MessageBkApi>();
builder.Services.AddScoped<ICheckInfoApi, CheckInfoApi>();
builder.Services.AddHostedService<BkWorker>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
