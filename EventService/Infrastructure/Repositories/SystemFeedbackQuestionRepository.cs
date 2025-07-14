using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SystemFeedbackQuestionRepository: GenericRepository<SystemFeedbackQuestion>, ISystemFeedbackQuestionRepository
    {
        public SystemFeedbackQuestionRepository(ScheduledEventServiceDbContext context) : base(context) { }

        public override async Task<List<SystemFeedbackQuestion>> GetAllAsync()
        {
            return await _context.SystemQuestionFeedbacks
                .Include(x => x.Answers)
                .ToListAsync();
        }
    }
}
