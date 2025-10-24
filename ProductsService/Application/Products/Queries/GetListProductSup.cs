using Application.Commons.Handlers;
using Application.Commons.PaginatedLists;
using Application.Commons.Results;
using Application.Commons.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Queries
{
    public class GetListProductSupQuery
    {
        public Guid idSup { get; set; }
    }
    public class ProductSummaryItems
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("package")]
        public List<RentalOption> Packages { get; set; }
        [JsonPropertyName("images")]
        public List<string> Images { get; set; }

    }

    public class GetProductListSupQueryHandler : IQueryHandler<GetListProductSupQuery, Result<List<ProductSummaryItems>>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductListSupQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<List<ProductSummaryItems>>> Handle(GetListProductSupQuery query, CancellationToken cancellationToken)
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync();

            var productsWith = products
                .Where(x => x.SupplierId == query.idSup)
                .ToList();


            // Return the 
            var items = productsWith.Select(x => new ProductSummaryItems
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.CategoryNavigation?.Name ?? "",
                ThumbnailUrl = x.ThumbnailUrl,
                Packages = x.ProductPackagesNavigation?.Select(p => new RentalOption
                {
                    PackageName = p.PackageStructureNavigation?.Name ?? "",
                    Price = p.Price
                }).ToList() ?? new(),
                Images = x.ImagesNavigation?.Select(img => img.ImageUrl).ToList() ?? new()
            }).ToList();

            return Result<List<ProductSummaryItems>>.Success(items);

        }
    }
}
