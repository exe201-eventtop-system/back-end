using Domain.Constants.UsedServices;
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
        Task<List<UsedService>> GetUsedServiceByUserIdAsync(Guid userd, UsedServiceStatus serviceStatus);
        Task<List<UsedService>> GetUsedServiceByUserIdSupAsync(Guid userd);
    }
}
