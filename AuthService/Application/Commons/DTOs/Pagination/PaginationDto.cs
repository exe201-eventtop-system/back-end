using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Pagination
{
   public class PaginationDto
    {
        [JsonPropertyName("page_number")]
        public int PageNumber { get; set; } = 1;

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; } = 12;
    }
}
