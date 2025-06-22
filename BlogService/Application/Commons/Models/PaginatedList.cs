using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.Models
{
    public class PaginatedList<T> where T : class
    {
        [JsonPropertyName("page_count")]
        public int PageCount { get; set; }

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

        //public PaginatedList(int current, int total, int page_size, IReadOnlyCollection<T> content)
        //{
        //    CurrentPage = current;
        //    TotalCount = total;
        //    PageSize = page_size;
        //    PageContent = content;
        //}
    }
}
