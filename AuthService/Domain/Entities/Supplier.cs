using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Supplier  : BaseEntities
    {
        [Column("id")]
        [ForeignKey(nameof(User))]
        public virtual Guid Id { get; set; }
        [Column("location_orginazation")]
        public string Location { get; set; } = string.Empty;
        [Column("name_organization")]
        public string NameOrginazation { get; set; } = string.Empty;
        [Column("tax_code")]
        public string TaxCode { get; set; } = string.Empty;
        [Column("is_active")]
        public bool IsActive { get; set; } = false;
        public virtual User Users { get; set; } = new User();
        public virtual ICollection<OrginazationImage> OrginazationImages { get; set; } = new List<OrginazationImage>();
    }
    
}
