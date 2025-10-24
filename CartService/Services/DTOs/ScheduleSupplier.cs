using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ScheduleSupplier
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ServiceDto Resource { get; set; } = new ServiceDto();
    }
    public class ServiceDto
    {
        public string ServiceName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Status { get; set; } 
        public string Image { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Supplier { get; set; } = string.Empty;
    }

}
