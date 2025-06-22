using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ServiceDTO
    {
        [JsonPropertyName("supplier_id")]
        public Guid SupllierId { get; set; } = Guid.Empty;
        [JsonPropertyName("supplier_name")]
        public string SupllierName { get; set; } 
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public List<RentalOptionDtoService> Packages { get; set; } = new List<RentalOptionDtoService>();
    }
    public class RentalOptionDtoService
    {
        [JsonPropertyName("package_name")]
        public int? PackageName { get; set; }
        public decimal? Price { get; set; }
        [JsonPropertyName("minimum_hours")]
        public int MinimumHours { get; set; }
        [JsonPropertyName("hourly_surcharge")]
        public decimal? HourlySurcharge { get; set; }
    }
}