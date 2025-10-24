using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using Microsoft.AspNetCore.Http;
using SharedLibrary.FireBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UploadProductImageCommand
    {
        [JsonPropertyName("thumbnail")]
        public IFormFile? Thumbnail { get; set; }
        [JsonPropertyName("images")]
        public List<IFormFile> Images { get; set; }

        [JsonIgnore]
        public Guid? ProductId { get; set; } = null;
    }

    public class UploadedImage
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }


    }

    public class UploadProductImageResult
    {
        [JsonPropertyName("images")]
        public List<UploadedImage> Images { get; set; }
    }

    public class UploadProductImageCommandHandler : ICommandHandler<UploadProductImageCommand, Result<UploadProductImageResult>>
    {
        private IUnitOfWork unitOfWork;
        private FirebaseStorageService firebaseStorageService;

        public UploadProductImageCommandHandler(IUnitOfWork uof, FirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = uof;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<Result<UploadProductImageResult>> Handle(UploadProductImageCommand command, CancellationToken cancellationToken)
        {
            // Lấy sản phẩm từ DB (bao gồm navigation ImagesNavigation)
            var product = await unitOfWork.ProductRepository.GetByIdsAsync(command.ProductId);

            if (product == null)
            {
                return Result<UploadProductImageResult>.Failure(
                    Application.Commons.Results.Error.NotFoundError($"Product with id {command.ProductId} not found"), 
                    "Product not found");
            }

            // Nếu có thumbnail -> upload
            if (command.Thumbnail != null)
            {
                string thumbnailUrl = await firebaseStorageService.Upload(command.Thumbnail);
                product.ThumbnailUrl = thumbnailUrl;
            }

            // Cập nhật UpdatedAt timestamp
            product.UpdatedAt = DateTime.UtcNow;

            // Update the product first (without navigation properties)
            var updatedProduct = await unitOfWork.ProductRepository.Update(product);

            // Nếu có ảnh sản phẩm -> xử lý riêng
            if (command.Images != null && command.Images.Any())
            {
                foreach (var formFile in command.Images)
                {
                    string imageUrl = await firebaseStorageService.Upload(formFile);

                    var image = new ProductImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = imageUrl,
                        ProductId = product.Id,
                    };

                    // Add image directly to the context using the repository
                    await unitOfWork.ProductRepository.CreateProductImageAsync(image);
                }
            }

            // Reload the product to get the updated data
            var finalProduct = await unitOfWork.ProductRepository.GetByIdsAsync(command.ProductId);

            // Trả kết quả
            var result = new UploadProductImageResult
            {
                Images = finalProduct.ImagesNavigation?.Select(x => new UploadedImage
                {
                    Id = x.Id,
                    Url = x.ImageUrl
                }).ToList() ?? new List<UploadedImage>()
            };

            return Result<UploadProductImageResult>.Success(result);
        }

    }
}
