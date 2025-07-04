using Microsoft.EntityFrameworkCore;
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
        public async Task<(long,Guid)> AddTransaction(Guid userId, int unitPrice)
        {
            string timePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"); 
            string randPart = new Random().Next(100, 999).ToString();
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("HHmmss")); // Giờ + phút + giây


            var transaction = new Transaction
            {
                UserId = userId,
                Amount = unitPrice,
                OrderCode = orderCode,
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return (transaction.OrderCode, transaction.Id);

        }
        public async Task<bool> SaveTransaction(long orderCode)
        {
            var transaction = await _context.Transactions
                .Include(t => t.UsedServices)
                .FirstOrDefaultAsync(t => t.OrderCode == orderCode);

            if (transaction == null)
                return false;

            transaction.IsPayment = true;

            var usedServiceIds = transaction.UsedServices
                .Where(us => !us.IsDeleted) 
                .Select(us => us.ServiceId)
                .Distinct()
                .ToList();

            var relatedCartItems = await _context.CartItems
                .Where(ci => usedServiceIds.Contains(ci.ServiceId) && !ci.IsDeleted)
                .ToListAsync();

            foreach (var cartItem in relatedCartItems)
            {
                cartItem.IsDeleted = true;
            }
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsTransactionPaidAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FindAsync(transactionId);
            return transaction?.IsPayment ?? false;
        }

    }
}
