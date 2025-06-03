using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class AuditableEntity<T>: BaseEntity<T> where T: struct
    {
        [Column("created_at", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column("last_modified_at", TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [Column("is_deleted", TypeName = "bit")]
        public bool IsDeleted { get; set; }
    }
}
