using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CartRespondeDTO
    {
        public List<CartItemResponse> Content { get; set; } = new();
        
    }
    public class CartItemResponse
    {
        public Guid? SupllierId { get; set; } = Guid.Empty;
        public Guid CartItem { get; set; } = Guid.Empty;
        public Guid ServiceId { get; set; } = Guid.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string SupllierName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;

        public List<RentalOptionDto> RentalOptions { get; set; } = new();
    }
    public class RentalOptionDto
    {
        [JsonPropertyName("package_name")]
        public string? PackageName{ get; set; }
        public decimal Price { get; set; }
        [JsonPropertyName("minimum_hours")]
        public int MinimumHours { get; set; }
        [JsonPropertyName("overtime_price")]
        public decimal? OvertimePrice { get; set; }

    }
}
