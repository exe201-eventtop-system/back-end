using Domain.Entities.Products;
using Domain.Enums;

namespace Domain.Entities.PackagesStructures
{
    public class PackageStructure
    {
        public Guid Id { get; set; }

        public Guid CreatorId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public PackageType Type { get; set; }

        public virtual List<Package> PackagesNavigation { get; set; }
    }
}
