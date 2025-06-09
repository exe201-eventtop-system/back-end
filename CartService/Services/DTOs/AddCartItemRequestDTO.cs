using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class AddCartItemRequestDTO
    {
        [JsonPropertyName("product_id")]
        public Guid ProductId { get; set; }
    }
}
