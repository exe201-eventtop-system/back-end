using Domain.Constants.UsedServices;
using SharedLibrary.System.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceFeedback : BaseEntities
    {
        [ForeignKey(nameof(Id))]
        [Column("id")]
        public virtual Guid Id { get; set; }
        [Column("rating_service")]
        public int RatingService { get; set; }  
        [Column("comment_service")]
        public string? CommentService { get; set; } 
        [Column("rating_supplier")]
        public int RatingSupplier { get; set; }
        [Column("comment_supplier")]
        public string? CommentSupplier { get; set; } 
        public virtual UsedService UsedService{ get; set; } = new UsedService();
    }
}
