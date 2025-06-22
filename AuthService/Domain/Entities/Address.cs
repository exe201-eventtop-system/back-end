using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Address  : BaseEntities
    {
        [Column("location")]
        public string Location { get; set; } = string.Empty;
        [Column("customer_id")]
        [ForeignKey(nameof(User))]
        public Guid CustomerId { get; set; }
        public virtual User?  Users { get; set; }

    }
}
