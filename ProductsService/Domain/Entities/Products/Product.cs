using Domain.Entities.Categories;
using Domain.Enums;
using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products
{
    public class Product : BaseEntities
    {
        [Column("name")]
        public string? Name { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("category_id")]
        public Guid CategoryId { get; set; }
        [Column("supplier_id")]
        public Guid SupplierId { get; set; }
        [Column("location")]
        public string? Location { get; set; }
        [Column("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        public ICollection<ProductImage>? ImagesNavigation { get; set; } = new List<ProductImage>();
        public virtual List<Package> ProductPackagesNavigation { get; set; }=  new List<Package>();
        [ForeignKey(nameof(CategoryId))]
        public virtual Category CategoryNavigation { get; set; } = new Category();

        public List<Package> GetProductPackageWithType(PackageType type)
        {
            return ProductPackagesNavigation.Where(x => x.PackageStructureNavigation.Type == type).ToList();
        }
    }
}
