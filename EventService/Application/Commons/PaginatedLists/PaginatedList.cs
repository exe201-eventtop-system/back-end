using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.PaginatedLists
{
    public class PaginatedList<T>
    {
        [JsonPropertyName("page_count")]
        public int PageCount => PageSize != 0 ? (int) Math.Ceiling((float)TotalCount / PageSize) : 0;

        [JsonPropertyName("current_page")]
        public int CurrentPage { get; set; }

        [JsonPropertyName("item_amount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }

        [JsonPropertyName("content")]
        public IReadOnlyCollection<T> PageContent { get; set; } = new List<T>();

        public bool HasNextPage => CurrentPage < PageCount;

        public bool HasPreviousPage => PageCount > 1;
    }
}
