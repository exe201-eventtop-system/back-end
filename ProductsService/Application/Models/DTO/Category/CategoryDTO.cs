using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.DTO.Category
{
    public class CategoryDTO
    {
        [JsonPropertyName("category_id")]
        public Guid Id { get; set; }
        [JsonPropertyName("category_name")]
        public string Name { get; set; }
        [JsonPropertyName("parent_property_id")]
        public Guid? ParentId { get; set; }
        [JsonPropertyName("parent_property_name")]
        public string? ParentName { get; set; }
        [JsonPropertyName("service_count")]
        public int ServiceCount { get; set; }
    }
}
