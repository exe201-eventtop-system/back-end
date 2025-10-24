using System.Text.Json.Serialization;

namespace Application.Commons.DTOs.Supplier
{
    public class SupplierDetailDTO
    {
        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("name_organization")]
        public string NameOrginazation { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("thumbnail")]
        public string Thumnnail { get; set; } = string.Empty;

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("images")]
        public List<string> OrginazationImages { get; set; } = new();
    }
}
