using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PackageStructureService: AuditableEntity<Guid>
    {
        [ForeignKey(nameof(PackageStructure))]
        public Guid StructureId { get; set; }
        [ForeignKey(nameof(Service))]
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }
        public int MinimumHours { get; set; }
        public decimal? HourlySurcharge { get; set; }
        public bool IsActive {  get; set; }

        public virtual PackageStructure PackageStructureNavigation { get; set; }
        public virtual Service ServicesNavigation { get; set; }

    }
}
