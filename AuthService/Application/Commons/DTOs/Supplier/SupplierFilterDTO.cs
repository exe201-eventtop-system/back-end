using Application.Commons.DTOs.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class SupplierFilterDto : PaginationDto
    {
        [JsonPropertyName("search_key")]
        public string? SearchKey { get; set; }

        [JsonPropertyName("min_rating")]
        public double? MinRating { get; set; }

        [JsonPropertyName("max_rating")]
        public double? MaxRating { get; set; }

        [JsonPropertyName("types")]
        public List<string>? Types { get; set; }

        [JsonPropertyName("address")]
        public string? Address { get; set; }
        [JsonPropertyName("lat")]
        public string? Lat { get; set; }
        [JsonPropertyName("lng")]
        public string? Lng { get; set; }

    }

}
