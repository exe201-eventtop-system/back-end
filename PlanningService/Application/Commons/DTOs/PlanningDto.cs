using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class PlanningDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public PlanningStatus? Status { get; set; }
        public string? Location { get; set; }
        public DateTime? DateOfEvent { get; set; }
        public decimal? Budget { get; set; }
        public int AboutNumberPeople { get; set; }
        public string? MainColor { get; set; }
        public string? TypeOfEvent { get; set; }

        public List<SesstionServiceDto>? SesstionServices { get; set; }
    }

}
