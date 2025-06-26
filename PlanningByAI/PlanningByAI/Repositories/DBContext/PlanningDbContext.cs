using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repositories.Models;

namespace Repositories.DBContext
{
    public class PlanningDbContext :  DbContext
    {
        public PlanningDbContext(DbContextOptions<PlanningDbContext> options) : base(options) { }

        public DbSet<Planning> Plannings { get; set; }
    }
}
