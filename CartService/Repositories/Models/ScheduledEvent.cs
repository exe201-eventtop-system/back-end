using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class ScheduledEvent : BaseModel
    {
        public Guid CustomerId { get; set; }
        public Guid SupplierId { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime AboutStartDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string AboutNumberPeople { get; set; } = string.Empty;
        public string TagMainColor { get; set; } = string.Empty;
        public int TypeOfEvent { get; set; }
        public int EventStatus { get; set; }
    }
}
