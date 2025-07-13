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
        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }

}
