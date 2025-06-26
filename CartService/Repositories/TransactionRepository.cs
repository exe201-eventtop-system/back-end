using Repositories.Basic;
using Repositories.DBContext;
using Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>
    {
        public TransactionRepository()
        {
        }
        public TransactionRepository(CartServiceDBContext context) => _context = context;
        public async Task<(long,Guid)> AddTransaction()
        {
            string timePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"); 
            string randPart = new Random().Next(100, 999).ToString();
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("HHmmss")); // Giờ + phút + giây


            var transaction = new Transaction
            {
                OrderCode = orderCode,
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return (transaction.OrderCode, transaction.Id);

        }
    }
}
