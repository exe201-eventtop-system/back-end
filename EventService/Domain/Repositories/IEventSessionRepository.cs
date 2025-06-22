using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Repositories
{
    public interface IEventSessionRepository: IGenericRepository<EventSession>
    {
        Task<EventSession?> GetByIdAsync(Guid id);
    }
}
