using Domain.Entities.Products;
using Domain.Enums;
using SharedLibrary.System.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.PackagesStructures
{
    public class PackageStructure : BaseEntities
    {
        [Column("name")]
        public string Name { get; set; }
        [Column("description")]
        public string Description { get; set; }
        [Column("type")]
        public PackageType Type { get; set; }

        public virtual List<Package> PackagesNavigation { get; set; }
    }
}
