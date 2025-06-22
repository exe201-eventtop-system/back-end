using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class BusyTimeDto
    {
        public string Start { get; set; } = string.Empty;
        public string End { get; set; } = string.Empty;
    }
 public class TimeSlotDto
    {
        public string Date { get; set; } = string.Empty;
        public List<BusyTimeDto> Busy { get; set; } = new List<BusyTimeDto>();
    }
   

}
