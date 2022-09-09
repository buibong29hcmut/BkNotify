using BkMail.Entities;
using Microsoft.EntityFrameworkCore;

namespace BkMail.Context
{
    public class BkDbContext:DbContext
    {
        public BkDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<MailSent> MailSents { get; set; }
        public DbSet<StudentData> StudentDatas { get; set; }
        public DbSet<EventData> EventDatas { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<StudentData>().Property(p => p.WsToken).HasMaxLength(100);
            builder.Entity<StudentData>().Property(p=>p.Email).HasMaxLength(100);
            builder.Entity<EventData>().Property(p => p.EventId).HasMaxLength(100);
            builder.Entity<EventData>().Property(p => p.WsToken).HasMaxLength(100);
            base.OnModelCreating(builder);
        }

    }
}
