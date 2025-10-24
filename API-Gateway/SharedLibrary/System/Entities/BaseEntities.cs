using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.System.Entities
{
    public abstract class BaseEntities
    {
        [Column("id")]
        public virtual Guid Id { get; set; } = Guid.NewGuid();
        [Column("create_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("update_at")]
        public DateTime UpdatedAt { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; } = false;
    }
}
