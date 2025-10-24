using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{   
    public class UsedServiceDto
    {
        [JsonPropertyName("unit_price")]
        public int UnitPrice { get; set; }
        [JsonPropertyName("services")]
        public List<Service>  Services { get; set; } = new List<Service>();
    }

    public class Service
    {
        [JsonPropertyName("event_id")]
        public Guid? EventId { get; set; }
        [JsonPropertyName("service_name")]
        public string ServiceName { get; set; } = string.Empty;
        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }
        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }
        [JsonPropertyName("rent_start_time")]
        public DateTime RentStartTime { get; set; }
        [JsonPropertyName("rent_end_time")]
        public DateTime RentEndTime { get; set; }
        [JsonPropertyName("price")]
        public int Price { get; set; }

    }
}
