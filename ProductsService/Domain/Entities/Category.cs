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
    public class Category: AuditableEntity<Guid>
    {
        [Column("name", Order = 1, TypeName = "NVARCHAR(64)")]
        public string Name { get; set; }

        [Column("description", Order = 2, TypeName = "NVARCHAR(256)")]
        public string Description { get; set; }

        [Column("parent_id", Order = 3, TypeName = "UNIQUEIDENTIFIER")]
        [ForeignKey(nameof(Category))]
        public Guid? ParentCategoryId { get; set; }

        public virtual Category? ParentCategoriesNavigation { get; set; }

        public virtual List<Category> ChildCategoriesNavigation { get; set; }

        public virtual List<Service> ServicesNavigation { get; set; }
    }
}
