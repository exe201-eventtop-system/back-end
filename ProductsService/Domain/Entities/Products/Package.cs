using Domain.Entities.PackagesStructures;
using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Products
{
    public class Package: BaseEntities
    {
        [Column("structure_id")]
        public Guid StructureId { get; set; }
        [Column("product_id")]
        public Guid ProductId { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("overtime_price")]
        public decimal OvertimePrice { get; set; }
        [Column("minimum_hour")]
        public int MinimumHour { get; set; }
        [ForeignKey(nameof(StructureId))]
        public virtual PackageStructure PackageStructureNavigation { get; set; }
        [ForeignKey(nameof(ProductId))]
        public virtual Product ProductNavigation { get; set; }
    }
}
