using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.UoW
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        IUsedServiceRepository UsedServiceRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ITransactionRepository TransactionRepository { get; }

        /// <summary>
        ///     Commit all changes made by repositories from this unit of work instance to persistent database.
        /// </summary>
        /// <exception cref="DbUpdateException"/>
        /// <exception cref="DbUpdateConcurrencyException"/>
        Task<int> CommitAsync();

        /// <summary>
        ///     Rollback all changes made by repositories from this unit of work instance.<br/>
        ///     Note: This only work if the changes are not commited.
        /// </summary>
        void Rollback();
    }
}
