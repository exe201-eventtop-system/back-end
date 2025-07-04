using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("sesstion_service")]
    public class SesstionService: BaseEntities
    {
        [Column("planning_id")]
        [ForeignKey(nameof(PlanningId))]
        public Guid PlanningId { get; set; }

        [Column("service_id")]
        public Guid? ServiceId { get; set; }

        public virtual Planning?  Planning { get; set; } 
    }
}
