using Domain.Entities;
using Domain.Enum;
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
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(ScheduledEventServiceDbContext context) : base(context)
        {
        }
        public async Task<(long, Guid)> AddTransaction(Guid userId, int unitPrice)
        {
            string timePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string randPart = new Random().Next(100, 999).ToString();
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("HHmmss"));


            var transaction = new Transaction
            {
                UserId = userId,
                PaymentType = PaymentType.CustomerPurcharse,
                Amount = unitPrice,
                IsPayment = true,
                OrderCode = orderCode,
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return (transaction.OrderCode, transaction.Id);

        }

        public async Task<(long, Guid)> AddTransactionReturnAmount(Guid userId, Guid transactionId, int unitPrice)
        {
            string timePart = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string randPart = new Random().Next(100, 999).ToString();
            int orderCode = int.Parse(DateTimeOffset.Now.ToString("HHmmss"));


            var transaction = new Transaction
            {
                UserId = userId,
                PaymentType = PaymentType.ReturnSupplier,
                Amount = unitPrice,
                OrderCode = orderCode,
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            return (transaction.OrderCode, transaction.Id);
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Transactions
                .Include(tr => tr.UsedServices)
                .Where(tr => tr.IsPayment == true && tr.IsDeleted == false)
                .ToListAsync();
        }


        public async Task<List<Guid>> SaveTransaction(long orderCode)
        {
            var transaction = await _context.Transactions
                .Include(t => t.UsedServices) 
                .FirstOrDefaultAsync(t => t.OrderCode == orderCode);

            if (transaction == null)
                return null;

            transaction.IsPayment = true;

            var serviceIds = transaction.UsedServices?
                .Select(us => us.ServiceId)
                .Distinct()
                .ToList();

            await _context.SaveChangesAsync();

            return serviceIds ?? new List<Guid>();
        }

    }
}
