using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
namespace Application.Commons.DTOs.Supplier
{

    public class SupplierDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("number_feedback")]
        public int NumberFeedback { get; set; }

        [JsonPropertyName("thumbnail")]
        public string Thumbnail { get; set; } = string.Empty;

        [JsonPropertyName("type_service")]
        public List<TypeOfServiceDto> TypeService { get; set; } = new();
    }

    public class TypeOfServiceDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
    }

}
