using Domain.Entities.PackagesStructures;

namespace Domain.Entities.Products
{
    public class Package
    {
        public Guid Id { get; set; }

        public Guid StructureId { get; set; }

        public Guid ProductId { get; set; }

        public decimal Price { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get ; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual PackageStructure PackageStructureNavigation { get; set; }
        public virtual Product ProductNavigation { get; set; }
    }
}
