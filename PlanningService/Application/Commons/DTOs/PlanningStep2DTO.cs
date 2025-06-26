using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs
{
    public class PlanningStep2DTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("location")]
        public string? Location { get; set; }

        [JsonPropertyName("date_of_event")]
        public DateTime? DateOfEvent { get; set; }

        [JsonPropertyName("budget")]
        public decimal? Budget { get; set; }

        [JsonPropertyName("about_number_people")]
        public int AboutNumberPeople { get; set; }

        [JsonPropertyName("main_color")]
        [MaxLength(50)]
        public string? MainColor { get; set; }

        [JsonPropertyName("type_of_event")]
        [MaxLength(50)]
        public string? TypeOfEvent { get; set; }
    }
}
