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

        [JsonPropertyName("alternative_text")]
        public string AlternativeText { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
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
            var item = await unitOfWork.ProductRepository.GetByIdAsync((Guid)command.ProductId);

            for (int i = 0; i < command.Images.Count; i++)
            {
                IFormFile current = command.Images[i];

                string url = await firebaseStorageService.Upload(current);

                ProductImage image = new ProductImage
                {
                    ImageUrl = url,
                    Order = item.ImagesNavigation.Count,
                    ProductId = (Guid) command.ProductId,
                };

                item.ImagesNavigation.Add(image);
            }

            item = await unitOfWork.ProductRepository.Update(item);

            return Result<UploadProductImageResult>.Success(new UploadProductImageResult
            {
                Images = item.ImagesNavigation.Select(x => new UploadedImage 
                { 
                    Id = x.Id, 
                    Url = x.ImageUrl, 
                    Order = x.Order,
                }).ToList()
            });
        }
    }
}
