using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetProductPackageCommand
    {
        public Guid ProductId { get; set; }
    }

    public class RentalPackageInfo
    {
        [JsonPropertyName("id")]
        public Guid PackageId { get; set; }

        [JsonPropertyName("name")]
        public string PackageName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("type")]
        public PackageType Type { get; set; }
    }

    public class GetProductPackageResult
    {
        [JsonPropertyName("packages")]
        public List<RentalPackageInfo> Packages { get; set; }
    }

    public class GetProductPackageQueryHandler : IQueryHandler<GetProductPackageCommand, Result<GetProductPackageResult>>
    {
        private IUnitOfWork unitOfWork;

        public GetProductPackageQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<GetProductPackageResult>> Handle(GetProductPackageCommand query, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.ProductRepository.GetByIdAsync(query.ProductId);

            if (item == null)
            {
                return Result<GetProductPackageResult>
                    .Failure(Error.NotFoundError($"Can not find service with id {query.ProductId}"), "Failed to fetch service detail");
            }

            return Result<GetProductPackageResult>.Success(new GetProductPackageResult
            {
                Packages = item.ProductPackagesNavigation.Select(x => new RentalPackageInfo
                {
                    PackageId = x.Id,
                    PackageName = x.PackageStructureNavigation.Name,
                    Type = x.PackageStructureNavigation.Type,
                    Price = x.Price,
                }).ToList()
            }, "Success");
        }
    }
}
