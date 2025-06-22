using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PackageStructure: AuditableEntity<Guid>
    {
        public Guid AdminId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public PackageType Type {  get; set; }

        public virtual List<PackageStructureService> PackagesNavigation { get; set; }
    }
}
