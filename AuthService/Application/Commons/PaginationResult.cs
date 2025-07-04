using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons
{
    public class PaginationResult<T> where T : class
    {
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("item_amount")]
        public int ItemCount { get; set; }

        [JsonPropertyName("content")]
        public required List<T> Items { get; set; }
    }
}
