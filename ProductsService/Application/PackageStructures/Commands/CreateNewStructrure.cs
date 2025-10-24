using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Entities.PackagesStructures;
using Domain.Entities.Products;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.PackageStructures.Commands
{
    public class CreatePackageStructureCommand
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public PackageType Type { get; set; }

        [JsonIgnore]
        public Guid AdminId { get; set; }
    }

    public class CreatePackageStructureResult
    {
        [JsonPropertyName("id")]
        public Guid PackageId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }
    }

    public class CreatePackageCommandHandler : ICommandHandler<CreatePackageStructureCommand, Result<CreatePackageStructureResult>>
    {
        private readonly IUnitOfWork unitOfWork;

        public CreatePackageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<CreatePackageStructureResult>> Handle(CreatePackageStructureCommand command, CancellationToken cancellationToken)
        {
            var newPackage = new PackageStructure
            {
                Name = command.Name,
                Description = command.Description,
                Type = command.Type,
            };

            newPackage = await unitOfWork.PackageStructureRepository.CreateAsync(newPackage);

            return Result<CreatePackageStructureResult>.Success(new CreatePackageStructureResult
            {
                PackageId = newPackage.Id,
                Name = newPackage.Name,
                Description = newPackage.Description,
            }, "Package created successfully");
        }
    }


}
