using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.DTO.Service
{
    public class ServiceSummaryDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("supplier_name")]
        public string SupplierName { get; set; }
        [JsonPropertyName("is_active")]
        public bool IsActive{ get; set; }
        [JsonPropertyName("packages")]
        public List<RentalOptionDto> RentalOptionListDto { get; set; } = new();
    }
    public class RentalOptionListDto
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
