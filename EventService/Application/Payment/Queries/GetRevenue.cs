using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Queries
{
    public class GetRevenueQuery { }
    public class RevenueDto
    {
        public string Month { get; set; } = default!;
        public decimal Revenue { get; set; }
    }
    public class GetRevenueHandler : IQueryHandler<GetRevenueQuery, Result<List<RevenueDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRevenueHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<RevenueDto>>> Handle(GetRevenueQuery query, CancellationToken cancellationToken)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllTransactionsAsync();

            var monthlyRevenue = transactions
                .Where(tr => tr.IsPayment && !tr.IsDeleted)
                .GroupBy(tr => tr.CreatedAt.ToString("yyyy-MM"))
                .Select(group => new RevenueDto
                {
                    Month = group.Key,
                    Revenue = group.Sum(tr => tr.Amount)
                })
                .OrderBy(r => r.Month)
                .ToList();

            return Result<List<RevenueDto>>.Success(monthlyRevenue);
        }
    }
}
