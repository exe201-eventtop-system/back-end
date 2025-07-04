using Repositories.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        CartItemRepository CartItemRepository { get; }
        CartRepository CartRepository { get; }
        UsedServiceRepository UsedServiceRepository { get; }
        TransactionRepository TransactionRepository { get; }
        int SaveChangesWithTransaction();

        Task<int> SaveChangesWithTransactionAsync();

    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly CartServiceDBContext _context;

        private CartItemRepository _cartItemRepository;
        private CartRepository _cartRepository;
        private UsedServiceRepository _usedServiceRepository;
        private TransactionRepository _transactionRepository;
        public UnitOfWork(CartServiceDBContext context)
        {
            _context = context;
        }

        public CartItemRepository CartItemRepository
        {
            get
            {
                return _cartItemRepository ??= new CartItemRepository(_context);
            }
        }
        public CartRepository CartRepository
        {
            get
            {
                return _cartRepository ??= new CartRepository(_context);
            }
        }

        public UsedServiceRepository UsedServiceRepository
        {
            get
            {
                return _usedServiceRepository ??= new UsedServiceRepository(_context);
            }
        }
        public TransactionRepository TransactionRepository
        {
            get
            {
                return _transactionRepository ??= new TransactionRepository(_context);
            }
        }
        public void Dispose() => _context.Dispose();

        public int SaveChangesWithTransaction()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = _context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }

        public async Task<int> SaveChangesWithTransactionAsync()
        {
            int result = -1;

            //System.Data.IsolationLevel.Snapshot
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    result = await _context.SaveChangesAsync();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    //Log Exception Handling message                      
                    result = -1;
                    dbContextTransaction.Rollback();
                }
            }

            return result;
        }


    }
}
