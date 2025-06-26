using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class PlanningResponseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime DateOfEvent { get; set; }
        public decimal Budget { get; set; }
        public string AboutNumberPeople { get; set; }
        public string MainColor { get; set; }
        public string TypeOfEvent { get; set; }
        public string GeneratedScript { get; set; }
    }
}
