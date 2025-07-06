using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using Domain.Repositories;
using System.Text.Json.Serialization;

namespace Application.Products.Queries
{
    public class GetProductInfoByIdQuery
    {
        [JsonPropertyName("id_list")]
        public List<Guid> ProductIdList { get; set; }
    }

    public class ProductPackageInformation
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("structure_id")]
        public Guid StructureId { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class ProductInformation
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }

        [JsonPropertyName("packages")]
        public List<ProductPackageInformation> ProductPackages { get; set; }
    }

    public class GetProductInfoByIdQueryHandler : IQueryHandler<GetProductInfoByIdQuery, Result<List<ProductInformation>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductInfoByIdQueryHandler(IUnitOfWork unitofwork)
        {
            this.unitOfWork = unitofwork;
        }

        public async Task<Result<List<ProductInformation>>> Handle(GetProductInfoByIdQuery query, CancellationToken cancellationToken)
        {
            List<ErrorDetail> errors = new List<ErrorDetail>();

            List<Product> productList = new List<Product>();

            foreach (Guid id in query.ProductIdList)
            {
                var product = await unitOfWork.ProductRepository.GetByIdAsync(id);

                if (product != null)
                {
                    productList.Add(product);
                }
                else
                {
                    errors.Add(new ErrorDetail(
                        Summary: "ServiceNotFound",
                        Detail: $"Can not find service detail with id {id}",
                        ErrorValue: id,
                        ErrorValueType: "NotFound"));
                }
            }

            if (errors.Count > 0)
            {
                return Result<List<ProductInformation>>
                    .Failure(Error.InvalidError("Some service seems to be invalid", errors),
                    "Failed while fetching service details");
            }

            return Result<List<ProductInformation>>.Success(productList.Select(x => new ProductInformation
            {
                Id = x.Id,
                Name = x.Name,
                SupplierId = x.SupplierId,
                ProductPackages = x.ProductPackagesNavigation.Select(y => new ProductPackageInformation
                {
                    Id = y.Id,
                    Name = y.PackageStructureNavigation.Name,
                    Price = y.Price,
                    StructureId = y.StructureId,
                }).ToList()
            }).ToList(), "Success");
        }
    }
}
