using Domain.Entities.Products;
using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities.Categories
{
    public class Category : BaseEntities
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }

        public virtual List<Product> ServicesNavigation { get; set; }
    }
}
