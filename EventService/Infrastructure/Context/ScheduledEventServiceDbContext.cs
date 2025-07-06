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

        public DbSet<EventType> EventTypes { get; set; }

        public DbSet<ScheduledEvent> Events { get; set; }

        public DbSet<UsedService> UsedSessionServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ScheduledEventServiceDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
