using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products
{
    public class ProductImage : BaseEntities
    {
        [Column("image_url")]

        public string? ImageUrl { get; set; }
        public Guid ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual Product? Product { get; set; }

    }
}
