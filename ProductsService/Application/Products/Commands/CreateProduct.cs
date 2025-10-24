using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using SharedLibrary.FireBase;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Products.Commands
{
    public class CreateProductCommand
    {
        [JsonPropertyName("name")]
        public string? ProductName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }

        [JsonIgnore]
        public Guid? SupplierId { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("packages")]
        public List<ProductPackage> Packages { get; set; }
        // Ex: [{"packageStructure":"E47188...","price":2000},{"packageStructure":"0C37...","price":300}]
    }

    public class ProductPackage
    {
        public Guid PackageStructure { get; set; }
        public decimal Price { get; set; }
        public decimal OvertimePrice { get; set; }
        public int MinimumHour { get; set; }
    }




    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<Guid>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly FirebaseStorageService firebaseStorageService;

        public CreateProductCommandHandler(
            IUnitOfWork unitOfWork,
            IHttpClientFactory httpClientFactory,
            FirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(command.CategoryId);

            // Create new Product entity
            var product = new Product
            {
                Name = command.ProductName,
                Description = command.Description,
                SupplierId = command.SupplierId.Value,
                CategoryNavigation = category,
                    CategoryId = command.CategoryId,
                Location = command.Location,
            };

                var createdProduct = await unitOfWork.ProductRepository.CreateAsync(product);

                // Create related Package entities
                foreach (var package in command.Packages)
                {
                    var newPackage = new Package
                    {
                        ProductId = createdProduct.Id,
                        Price = package.Price,
                        MinimumHour = package.MinimumHour,
                        OvertimePrice = package.OvertimePrice,
                        StructureId = package.PackageStructure
                    };

                    await unitOfWork.PackageRepository.CreateAsync(newPackage);
                }


                return Result<Guid>.Success(product.Id, "Successfully created product");
            
            
        }
    }

}

