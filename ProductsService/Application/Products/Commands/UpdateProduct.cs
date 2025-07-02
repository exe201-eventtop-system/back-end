using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Application.Products.Queries;
using Microsoft.AspNetCore.Http;
using SharedLibrary.FireBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class UpdateProductCommand
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public IFormFile Thumbnail { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("category")]
        public Guid Category { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateProductResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("thumbnail")]
        public IFormFile? Thumbnail { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonIgnore]
        public Guid Id { get; set; }
    }

    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand, Result<UpdateProductResult>>
    {
        private IUnitOfWork unitOfWork;
        private FirebaseStorageService firebaseStorageService;

        public UpdateProductCommandHandler(IUnitOfWork unitOfWork, FirebaseStorageService firebaseStorageService)
        {
            this.unitOfWork = unitOfWork;
            this.firebaseStorageService = firebaseStorageService;
        }

        public async Task<Result<UpdateProductResult>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            var item = await unitOfWork.ProductRepository.GetByIdAsync(command.Id);

            if (item == null)
            {
                return Result<UpdateProductResult>
                    .Failure(Error.NotFoundError($"Can not find service with id {command.Id}"), "Failed while processing request");
            }

            // Validate input đầu vào (chưa implement)
            List<ErrorDetail> errors = new List<ErrorDetail>();

            // if (command.Name.IsNullOrEmpty() ) { ... }

            if (errors.Any())
            {
                return Result<UpdateProductResult>.Failure(Error.InvalidError("Validation failed while updating service", errors), "Failed while processing request");
            }

            if (command.Thumbnail != null) 
            {
                item.ThumbnailUrl = await firebaseStorageService.Upload(command.Thumbnail);
            }
            item.Name = command.Name;
            item.Description = command.Description;
            item.Location = command.Location;
            item.CategoryId = command.Category;

            item = await unitOfWork.ProductRepository.Update(item);

            return Result<UpdateProductResult>.Success(new UpdateProductResult
            {
                Id = command.Id,
                Description = command.Description,
                Name = command.Name,
                Thumbnail = command.Thumbnail,
                Location = command.Location,
            });
        }
    }
}
