using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("planning")]
    public class Planning  : BaseEntities
    {
        [Required]
        [Column("customer_id")]
        public Guid CustomerId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("name")]
        public string Name { get; set; }

        [Column("description", TypeName = "text")]
        public string? Description { get; set; }

        [Column("status")]
        public PlanningStatus? Status { get; set; } = PlanningStatus.Draft;

        [Column("location", TypeName = "text")]
        public string? Location { get; set; }

        [Column("date_of_event")]
        public DateTime? DateOfEvent { get; set; }

        [Column("budget", TypeName = "decimal(10,2)")]
        public decimal? Budget { get; set; }

        [Column("about_number_people")]
        public int AboutNumberPeople { get; set; }

        [MaxLength(50)]
        [Column("main_color")]
        public string? MainColor { get; set; }

        [MaxLength(50)]
        [Column("type_of_event")]
        public string? TypeOfEvent { get; set; }

        public ICollection<SesstionService>? SesstionServices { get; set; }
    }
}
