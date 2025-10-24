using Application.Commons.Handlers;
using Application.Commons.Results;
using Application.Commons.UoW;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.PackageStructures.Queries
{
    public class GetStructureListCommand
    {
    }

    public class FlattenedPackageStructure
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public PackageType Type { get; set; }
    }

    public class GetStructureListResult
    {
        [JsonPropertyName("structures")]
        public List<FlattenedPackageStructure> Structures { get; set; }
    }

    public class GetStructureListQueryHandler : IQueryHandler<GetStructureListCommand, Result<GetStructureListResult>>
    {
        private readonly IUnitOfWork unitOfWork;

        public GetStructureListQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<GetStructureListResult>> Handle(GetStructureListCommand command, CancellationToken cancellationToken)
        {
            var structures = await unitOfWork.PackageStructureRepository.GetAllAsync();

            if (structures == null || !structures.Any())
            {
                return Result<GetStructureListResult>
                    .Failure(Error.NotFoundError("No package structures found"), "No structures found");
            }

            var mapped = structures.Select(s => new FlattenedPackageStructure
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                Type = s.Type,
            }).ToList();

            return Result<GetStructureListResult>.Success(new GetStructureListResult
            {
                Structures = mapped
            }, "Success");
        }
    }

}
