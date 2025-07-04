using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class BookingHistoryDTO
    {
        public Guid ServiceId { get; set; } = Guid.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string SupllierName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime StartTime { get; set; } = DateTime.MinValue;
        public DateTime EndTime { get; set; } = DateTime.MinValue;
        public DateOnly Date { get; set; }
    }
}
