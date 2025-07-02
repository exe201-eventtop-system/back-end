using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.ProductCategories.Queries
{
    public class GetProductCategoryListCommand
    {
    }

    public class FlattenedProductCategory
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parent_id")]
        public Guid? ParentId { get; set; }
    }

    public class GetProductCategoryListResult
    {
        [JsonPropertyName("categories")]
        public List<FlattenedProductCategory> Categories { get; set; }
    }

    public class GetProductCategoryListQueryHandler : IQueryHandler<GetProductCategoryListCommand, Result<GetProductCategoryListResult>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetProductCategoryListQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<GetProductCategoryListResult>> Handle(GetProductCategoryListCommand query, CancellationToken cancellationToken)
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();

            if (categories == null || !categories.Any())
            {
                return Result<GetProductCategoryListResult>
                    .Failure(Error.NotFoundError("No product categories found"), "No categories found");
            }

            var flattened = categories.Select(c => new FlattenedProductCategory
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentId = c.ParentCategoryId
            }).ToList();

            var result = new GetProductCategoryListResult
            {
                Categories = flattened
            };

            return Result<GetProductCategoryListResult>.Success(result, "Successfully retrieved category list");
        }
    }
}
