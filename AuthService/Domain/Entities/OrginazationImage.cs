using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrginazationImage : BaseEntities
    {
        [Column("image_url")]
        public  string? ImageUrl { get; set; }
        [Column("supplier_id")]
        [ForeignKey(nameof(Supplier))]
        public Guid SupplierId { get; set; }
        public virtual Supplier? Supplier { get; set; }
    }
}
