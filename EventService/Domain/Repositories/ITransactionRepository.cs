using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.Repositories
{
    public interface ITransactionRepository : IGenericRepository<Transaction>
    {
        Task<(long, Guid)> AddTransaction(Guid userId, int unitPrice);
        Task<List<Guid>> SaveTransaction(long orderCode);
    }
}
