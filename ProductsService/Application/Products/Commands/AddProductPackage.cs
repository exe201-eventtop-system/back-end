using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Products.Commands
{
    public class AddProductPackageCommand
    {
        [JsonPropertyName("structure")]
        public Guid PackageStructure {  get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonIgnore]
        public Guid? ProductId { get; set; } = null;
    }

    public class AddProductPackageResult
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("structure")]
        public Guid PackageStructure { get; set; }

        [JsonPropertyName("structure_name")]
        public string PackageStructureName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class AddProductPackageHandler : ICommandHandler<AddProductPackageCommand, Result<AddProductPackageResult>>
    {
        private IUnitOfWork unitOfWork;

        public AddProductPackageHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<AddProductPackageResult>> Handle(AddProductPackageCommand command, CancellationToken cancellationToken)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync((Guid) command.ProductId);

            if (product == null)
            {
                return Result<AddProductPackageResult>
                    .Failure(Error.NotFoundError($"Can not find service with id {command.ProductId}"), "Failed while processing request");
            }

            var packageStructure = await unitOfWork.PackageStructureRepository.GetByIdAsync(command.PackageStructure);

            if (packageStructure == null)
            {
                return Result<AddProductPackageResult>
                    .Failure(Error.NotFoundError($"Can not find structure with id {command.PackageStructure}"), "Failed while processing request");
            }

            var result = await unitOfWork.PackageRepository.CreateAsync(new Package
            {
                StructureId = command.PackageStructure,
                Price = command.Price,
                ProductId = product.Id,
                IsActive = true
            });

            return Result<AddProductPackageResult>.Success(new AddProductPackageResult
            {
                Id = result.Id,
                PackageStructureName = packageStructure.Name,
                PackageStructure = result.StructureId,
                Price = result.Price,
            }, "Successfully created package for service");
        }
    }
}
