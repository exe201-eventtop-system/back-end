using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Context
{
    public class ScheduledEventServiceDbContext: DbContext
    {
        public ScheduledEventServiceDbContext(DbContextOptions<ScheduledEventServiceDbContext> options) : base(options) { }


        public DbSet<ScheduledEvent> Events { get; set; }

        public DbSet<UsedService> UsedServices { get; set; }
        public DbSet<ServiceFeedback> Feedbacks { get; set; }
        public DbSet<SystemFeedbackAnswer> Answers{ get; set; }
        public DbSet<SystemFeedbackQuestion> SystemQuestionFeedbacks{ get; set; }
        public DbSet<Transaction> Transactions{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ServiceFeedback>()
    .HasOne(f => f.UsedService)
    .WithOne(us => us.Feedback)
    .HasForeignKey<ServiceFeedback>(f => f.Id);

        }
    }
}
