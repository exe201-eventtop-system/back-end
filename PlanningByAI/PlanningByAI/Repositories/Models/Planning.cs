using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repositories.Models
{
    public class Planning
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateOfEvent { get; set; }
        public decimal Budget { get; set; }
        [MaxLength(50)]
        public string AboutNumberPeople { get; set; }
        public string MainColor { get; set; }
        public string TypeOfEvent { get; set; }

        public string? GeneratedScript { get; set; } // 🆕 Thêm chỗ lưu kịch bản AI

        public DateTime CreateAt { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
