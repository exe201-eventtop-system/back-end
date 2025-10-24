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
    public class UpdateProductImageCommand
    {
        [JsonPropertyName("images")]
        public List<IFormFile> Images { get; set; }

        public Guid? ProductId { get; set; } = null;
    }

    public class UpdatedImage
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("alternative_tex")]
        public string AlternativeText { get; set; }

        [JsonPropertyName("order")]
        public int Order { get; set; }
    }

    public class UpdatedProductImageResult
    {
        [JsonPropertyName("image")]
        public List<UpdatedImage> Images { get; set; }
    }

    public class UpdateProductImageCommandHandler : ICommandHandler<UpdateProductImageCommand, Result<UpdatedProductImageResult>>
    {
        private IUnitOfWork unitOfWork;
        private FirebaseStorageService firebaseStorageService;

        public UpdateProductImageCommandHandler(IUnitOfWork uof, FirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = uof;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<Result<UpdatedProductImageResult>> Handle(UpdateProductImageCommand command, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.ProductRepository.GetByIdAsync((Guid) command.ProductId);

            if (item == null)
            {
                return Result<UpdatedProductImageResult>.Failure(Error.NotFoundError($"Can not find service with id {command.ProductId}"), "Error while processing request.");
            }

            item.ImagesNavigation.Clear();

            for (int i = 0; i < command.Images.Count; i++)
            {
                IFormFile current = command.Images[i];

                string url = await firebaseStorageService.Upload(current);

                ProductImage image = new ProductImage
                {
                    ImageUrl = url,
                    ProductId = (Guid) command.ProductId,
                };

                item.ImagesNavigation.Add(image);
            }

            item = await unitOfWork.ProductRepository.Update(item);

            return Result<UpdatedProductImageResult>.Success(new UpdatedProductImageResult
            {
                Images = item.ImagesNavigation.Select(x => new UpdatedImage
                {
                    Id = x.Id,
                    Url = x.ImageUrl,
                }).ToList()
            });
        }
    }
}
