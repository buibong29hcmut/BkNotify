using BkMail.Context;
using BkMail.Contracts;
using BkMail.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace BkMail.Pages
{
    public partial class Index
    {
        [Inject]   
        private ISnackbar SackBar { get; set; }
        [Inject]
        private IDbContextFactory<BkDbContext> _Factory { get; set; }
        [Inject]
        private ICheckInfoApi checkInfoApi { get; set; }
        [Inject]
        private ILogger<Index> _logger { get; set; }
        private string Email { get; set; }
        private string WsToken { get; set; }
        
        public void ShowMessage(string message, Severity severity)
        {
            SackBar.Add(message,severity);
        }
        public async Task Submit()
        {
            if(string.IsNullOrEmpty(Email)&& string.IsNullOrEmpty(WsToken))
            {
                ShowMessage("Email và WsToken không được để trống", Severity.Normal);
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                ShowMessage("Email  không được để trống", Severity.Warning);
                return;
            }
            if (string.IsNullOrEmpty(WsToken))
            {
                ShowMessage(" WsToken không được để trống", Severity.Warning);
                return;
            }
            bool check = await checkInfoApi.Check(WsToken);
            if (!check)
            {
                ShowMessage(" WsToken không đúng", Severity.Warning);

            }
            using(var db= _Factory.CreateDbContext())
            {
                await db.AddAsync(new StudentData(WsToken, Email));
                await db.SaveChangesAsync();
                _logger.LogInformation("Add user");
            }
        }
    }
}
