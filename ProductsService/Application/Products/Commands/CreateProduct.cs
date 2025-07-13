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
        public string ProductName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public IFormFile? Thumbnail { get; set; }

        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }

        [JsonPropertyName("parent_id")]
        public Guid? ParentId { get; set; } = null;

        [JsonIgnore]
        public Guid? SupplierId { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("packages")]
        public string Packages { get; set; } // Ex: [{"packageStructure":"E47188...","price":2000},{"packageStructure":"0C37...","price":300}]
    }

    public class ProductPackage
    {
        public Guid PackageStructure { get; set; }

        public decimal Price { get; set; }
    }

    public class CreateProductResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail_url")]
        public string Thumbnail { get; set; }
    }

    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Result<CreateProductResult>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly FirebaseStorageService firebaseStorageService;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IHttpClientFactory httpClientFactory, FirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = unitOfWork;
            this.httpClientFactory = httpClientFactory;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<Result<CreateProductResult>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Save thumbnail image to storage and get the link to that image.
            var storage_link = command.Thumbnail != null ? await firebaseStorageService.Upload(command.Thumbnail) : "";

            Product? result;

            try
            {

                Console.WriteLine(command.Packages);
                // Parse package info from string.
                List<ProductPackage>? packages = JsonSerializer.Deserialize<List<ProductPackage>>(command.Packages, new JsonSerializerOptions
                    { AllowTrailingCommas = true });

                if (packages == null)
                {
                    throw new JsonException($"Can not parse the given packages (string: {command.Packages})");
                }

                // Add service detail
                result = await unitOfWork.ProductRepository.CreateAsync(new Product
                {
                    Name = command.ProductName,
                    Description = command.Description,
                    SupplierId = (Guid)command.SupplierId,
                    ThumbnailUrl = storage_link,
                    CategoryId = command.CategoryId,
                    Location = command.Location,
                });

                foreach (var package in packages)
                {

                    await unitOfWork.PackageRepository.CreateAsync(new Package
                    {
                        ProductId = result.Id,
                        Price = package.Price,
                        StructureId = package.PackageStructure
                    });
                }
                
            }
            catch (Exception ex)
            {
                ErrorDetail detail = new ErrorDetail(ex.GetType().Name, ex.Message, null, ex.GetType().FullName);

                return Result<CreateProductResult>
                    .Failure(Error.UnhandledError("", new List<ErrorDetail> { detail }), "Failed while saving service details");
            }

            return Result<CreateProductResult>.Success(new CreateProductResult
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                Thumbnail = result.ThumbnailUrl,
            }, "Successfully created service");
        }
    }
}
