using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class DeleteCartDTO
    {
        [JsonPropertyName("id_cart_item")]
        public Guid CartId { get; set; } = Guid.Empty;
    }
}
