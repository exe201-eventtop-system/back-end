using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Application.Feedbacks.Queries;
using Microsoft.Extensions.Configuration;
using SharedLibrary.DTOs.Supplier;
using SharedLibrary.System.APICall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Queries
{
    public class GetTransactionQuery { }
    public class TransactionDTOs
    {
        public long OrderCode { get; set; }
        public string? CustomerName { get; set; }
        public decimal? Price { get; set; }
        public bool? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Transactionitem>? TransactionItems { get; set; }
    }
    public class Transactionitem
    {
        public string? SupplierName { get; set; }
        public string? ServiceName { get; set; }
        public decimal? UnitPrice { get; set; }
        public DateTime CreatedAt { get; set; }

    }

    public class GetTransactionHandler : IQueryHandler<GetTransactionQuery, Result<List<TransactionDTOs>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTransactionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<TransactionDTOs>>> Handle(GetTransactionQuery query, CancellationToken cancellationToken)
        {
            var transactions = await _unitOfWork.TransactionRepository.GetAllTransactionsAsync();

            var result = transactions.Select(tr => new TransactionDTOs
            {
                OrderCode = tr.OrderCode,
                CreatedAt = tr.CreatedAt,
                Price = tr.Amount,
                Status = tr.IsPayment,
                CustomerName = tr.UsedServices?.FirstOrDefault()?.CustomerName.ToString(),
                TransactionItems = tr.UsedServices?.Select(us => new Transactionitem
                {
                    SupplierName = us.SupplierId.ToString(),
                    ServiceName = us.ServiceName,
                    UnitPrice = us.UnitPrice,
                    CreatedAt = us.CreatedAt
                }).ToList()
            }).ToList();

            return Result<List<TransactionDTOs>>.Success(result);
        }
    }

}
