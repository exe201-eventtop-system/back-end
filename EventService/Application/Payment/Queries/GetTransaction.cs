using Application.Commons.Handlers;
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
    public class GetTransactionQuery : List<TransactionDTOs> { }
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

    //public class GetTransactionHandler : IQueryHandler<GetTransactionQuery, List<TransactionDTOs>>
    //{
    //    private readonly IUnitOfWork _unitOfWork;
    //    private readonly ApiCaller _apiCaller;
    //    private readonly IConfiguration _config;

    //    public GetTransactionHandler(IUnitOfWork unitOfWork,IConfiguration  configuration,ApiCaller  apiCaller)
    //    {
    //        _unitOfWork = unitOfWork;
    //        _config = configuration;
    //        _apiCaller = apiCaller;
    //    }

        //public async Task<List<TransactionDTOs>> Handle(GetTransactionQuery query, CancellationToken cancellationToken)
        //{
            //var baseUrlAuth = _config["AUTHSERVICE:PORT"];
            //var baseUrlProduct = _config["PRODUCTSERVICE:PORT"];
            //var urlAuth = $"{baseUrlAuth}/api/feeback/supplier-rating";


            //var transactions = await _unitOfWork.UsedServiceRepository.GetAllAsync();

            //if (transactions == null || !transactions.Any())
            //    return null;

            //var result = new List<TransactionDTOs>();

            //foreach (var transaction in transactions)
            //{
            //    var urlProduct = $"{baseUrlProduct}/api/api/services/{transaction.UsedServiceTransactions.}";
            //    var firstUsedService = transaction.UsedServices.FirstOrDefault();
            //    if (firstUsedService == null) continue;

            //    var customer = await _serviceClient.GetCustomerByIdAsync(firstUsedService.CustomerId);
            //    var transactionItems = new List<Transactionitem>();

            //    foreach (var item in transaction.UsedServices)
            //    {
            //        var service = await _serviceClient.GetServiceByIdAsync(item.ServiceId);

            //        transactionItems.Add(new Transactionitem
            //        {
            //            ServiceName = service?.Name ?? item.ServiceName,
            //            SupplierName = service?.Supplier.SupplierName ?? "Không tìm thấy",
            //            UnitPrice = item.UnitPrice,
            //            CreatedAt = item.CreatedAt
            //        });
            //    }

            //    result.Add(new TransactionDTOs
            //    {
            //        OrderCode = transaction.OrderCode,
            //        CustomerName = customer.CustomerName ?? "Không rõ",
            //        Price = transaction.Amount,
            //        Status = firstUsedService.Transaction.IsPayment,
            //        CreatedAt = transaction.CreatedAt,
            //        TransactionItems = transactionItems
            //    });
            //}

            //return ServiceResult<List<TransactionDTOs>>.Success(result);
       // }


    //}
}
