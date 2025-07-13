using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Categories;
using System.Text.Json.Serialization;

namespace Application.ProductCategories.Commands
{
    public class CreateNewCategoryCommand
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("parent")]
        public Guid? ParentId { get; set; } = null;
    }

    public class CreateCategoryResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class CreateCategoryCommandHandler : ICommandHandler<CreateNewCategoryCommand, Result<CreateCategoryResult>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateCategoryResult>> Handle(CreateNewCategoryCommand command, CancellationToken cancellationToken)
        {
            Category parentCategory;
            if (command.ParentId != null)
            {
                parentCategory = await unitOfWork.CategoryRepository.GetByIdAsync(command.ParentId);

                if (parentCategory == null)
                {
                    return Result<CreateCategoryResult>
                        .Failure(Error.NotFoundError($"Parent category with ID {command.ParentId} does not exist."), "Failed to create category");
                }
            }

            var newCategory = new Category
            {
                Name = command.Name,
                Description = command.Description,
            };

            newCategory = await unitOfWork.CategoryRepository.CreateAsync(newCategory);

            var result = new CreateCategoryResult
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            };

            return Result<CreateCategoryResult>.Success(result, "Category created successfully");
        }
    }

}
