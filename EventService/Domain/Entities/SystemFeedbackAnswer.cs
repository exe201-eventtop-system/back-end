using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SystemFeedbackAnswer  : BaseEntities
    {
        [Column("question_id")]
        public Guid QuestionId { get; set; }  
        [Column("answer_text")]
        public string AnswerText { get; set; } = string.Empty;
        [Column("customer_id")]
        public Guid  CustomerId { get; set; }
        [Column("customer_name")]
        public string CustomerName { get; set; }

        [ForeignKey(nameof(QuestionId))]
        public virtual SystemFeedbackQuestion SystemFeedback { get; set; }
    }
}
