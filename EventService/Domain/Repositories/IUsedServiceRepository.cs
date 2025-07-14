using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IUsedServiceRepository: IGenericRepository<UsedService>
    {
        Task<List<UsedService>> GetUsedServicesWithRating();
        Task<List<UsedService>> GetScheduleIdAsync(Guid supplierId);
    }
}
