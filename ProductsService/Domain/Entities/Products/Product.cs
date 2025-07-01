using Domain.Entities.Categories;
using Domain.Enums;

namespace Domain.Entities.Products
{
    public class Product
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ParentServiceId { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid SupplierId { get; set; }

        public string Location { get; set; }

        public string ThumbnailUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual List<ProductImage> ImagesNavigation { get; set; }
        public virtual List<Package> ProductPackagesNavigation { get; set; }
        public virtual Category CategoryNavigation { get; set; }
        public virtual Product ParentServiceNavigation { get; set; }
        public virtual List<Product> ChildServicesNavigation { get; set; }

        public List<Package> GetProductPackageWithType(PackageType type)
        {
            return ProductPackagesNavigation.Where(x => x.PackageStructureNavigation.Type == type).ToList();
        }
    }
}
