using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SystemFeedbackQuestion : BaseEntities
    {
        [Column("question")]
        public string Question { get; set; } = string.Empty;
        public virtual ICollection<SystemFeedbackAnswer>? Answers { get; set; }
    }
}
