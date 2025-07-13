using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Service
{
    public class ServiceRatingDto
    {
        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }
        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }
    }
}
