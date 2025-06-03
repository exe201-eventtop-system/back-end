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
    public class ServiceImage: AuditableEntity<Guid>
    {
        public required string ImageUrl { get; set; }

        [ForeignKey(nameof(Service))]
        public required Guid ServiceId { get; set; }

        public virtual Service ServiceNavigation { get; set; }
    }
}
