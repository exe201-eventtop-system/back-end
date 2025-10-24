using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IEventRepository: IGenericRepository<ScheduledEvent>
    {
    }
}
