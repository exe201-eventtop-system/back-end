using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class SystemFeedbackAnswerRepository: GenericRepository<SystemFeedbackAnswer>, ISystemFeedbackAnswerRepository
    {
        public SystemFeedbackAnswerRepository(ScheduledEventServiceDbContext context) : base(context) { }
    }
}
