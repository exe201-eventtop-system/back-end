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
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<UsedServiceTransaction> UsedServiceTransactions { get; set; }
        public DbSet<Answer> Answers{ get; set; }
        public DbSet<SystemQuestionFeedback> SystemQuestionFeedbacks{ get; set; }
        public DbSet<Transaction> Transactions{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Feedback>()
    .HasOne(f => f.UsedService)
    .WithOne(us => us.Feedback)
    .HasForeignKey<Feedback>(f => f.Id);

        }
    }
}
