using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class ActionServiceDTO
    {
        [JsonPropertyName("planning_id")]
        public Guid PlanningId { get; set; }
        [JsonPropertyName("service_id")]
        public Guid ServiceId { get; set; }
    }
}
