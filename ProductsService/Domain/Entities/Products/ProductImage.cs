using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products
{
    public class ProductImage : BaseEntities
    {
        [Column("order")]
        public int Order { get; set; }
        [Column("image_url")]

        public string ImageUrl { get; set; }
        public Guid ProductId { get; set; }
    }
}
