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
    public class Service: AuditableEntity<Guid>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [ForeignKey(nameof(Service))]
        public Guid? ParentServiceId { get; set; }

        [ForeignKey(nameof(Category))]
        public Guid CategoryId { get; set; }

        public Guid SupplierId { get; set; }

        public string Location { get; set; }

        public string ThumbnailUrl { get; set; }

        public virtual List<ServiceImage> ImagesNavigation { get; set; }
        public virtual List<PackageStructureService> PackageStructureServiceNavigation { get; set; }
        public virtual Category CategoryNavigation { get; set; }
        public virtual Service ParentServiceNavigation { get; set; }
        public virtual List<Service> ChildServicesNavigation { get; set; }
    }
}
