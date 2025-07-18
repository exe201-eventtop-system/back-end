using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using SharedLibrary.DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class ProductMinimalInformationQuery
    {
    }

    public class SupplierProductMinimalInformationQuery
    {
        public Guid Id { get; set; }
    }

    public class GetProductMinimalInformationHandler : IQueryHandler<ProductMinimalInformationQuery, Result<List<MinimalServiceInfo>>>, IQueryHandler<SupplierProductMinimalInformationQuery, Result<List<MinimalServiceInfo>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductMinimalInformationHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<MinimalServiceInfo>>> Handle(ProductMinimalInformationQuery query, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.ProductRepository.GetAllAsync();

            return Result<List<MinimalServiceInfo>>
                .Success(result.Select(x => new MinimalServiceInfo(x.Id, x.SupplierId, x.Name, x.CreatedAt, x.IsDeleted)).ToList(), "Success");
        }

        public async Task<Result<List<MinimalServiceInfo>>> Handle(SupplierProductMinimalInformationQuery query, CancellationToken cancellationToken)
        {
            var result = await unitOfWork.ProductRepository.GetAllAsync(x => x.SupplierId == query.Id, null);

            return Result<List<MinimalServiceInfo>>
                .Success(result.Select(x => new MinimalServiceInfo(x.Id, x.SupplierId, x.Name, x.CreatedAt, x.IsDeleted)).ToList());
        }
    }
}
