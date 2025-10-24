using Domain.Enum;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class PlanningFilterDTO
    {
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "size")]
        public int Size { get; set; } = 10;

        [FromQuery(Name = "status")]
        public PlanningStatus Status { get; set; } = PlanningStatus.Draft;

        [FromQuery(Name = "search")]
        public string? Search { get; set; }
    }
}
