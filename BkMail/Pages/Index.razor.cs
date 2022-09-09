using BkMail.Context;
using BkMail.Contracts;
using BkMail.Entities;
using Blazorise;
using Blazorise.Snackbar;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BkMail.Pages
{
    public partial class Index
    {
        private Snackbar validateTokenAndEmail;
        private Snackbar snackbarValidateEmail;
        private Snackbar snackbarValidateToken;
        private Snackbar validateWsTokenValid;
        private Snackbar emailValid;
        private Snackbar validateExistWsTokenAndEmail;
        private Snackbar success;
        [Inject]
        private IDbContextFactory<BkDbContext> _Factory { get; set; }
        [Inject]
        private ICheckInfoApi checkInfoApi { get; set; }
        [Inject]
        private ILogger<Index> _logger { get; set; }
        private string Email { get; set; }
        private string WsToken { get; set; }
        
        public void ShowMessage( Snackbar snackbar)
        {
            snackbar.Show();
        }
        public async Task Submit()
        {
            if(string.IsNullOrEmpty(Email)&& string.IsNullOrEmpty(WsToken))
            {
                ShowMessage(validateTokenAndEmail);
                return;
            }
            if (string.IsNullOrEmpty(Email))
            {
                ShowMessage(snackbarValidateEmail);

                return;
            }
            if (string.IsNullOrEmpty(WsToken))
            {
                ShowMessage(snackbarValidateToken);
                return;
            }
            var trimmedEmail = Email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                ShowMessage(validateWsTokenValid);

            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(Email);
            }
            catch
            {
                ShowMessage(validateWsTokenValid);
                return;
            }
            bool check = await checkInfoApi.Check(WsToken);
            if (!check)
            {
                ShowMessage(validateWsTokenValid);
                return;

            }
         
            using (var db = _Factory.CreateDbContext())
            {
                if(db.StudentDatas.Any(p=>p.WsToken==WsToken&& p.Email==Email))
                {
                    ShowMessage(validateExistWsTokenAndEmail);
                    return;
                } 
                await db.AddAsync(new StudentData(WsToken, Email));
                await db.SaveChangesAsync();
                ShowMessage(success);

                _logger.LogInformation("Add user");
            }
        }
    }
}
