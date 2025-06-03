using Domain.Entities;

namespace Infrastructure.Repositories.Contracts
{
    public interface IServiceRepository : IGenericRepository<Service, Guid>
    {
    }
}
