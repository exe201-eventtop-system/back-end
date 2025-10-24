using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class AddCartItemResponseDTO
    {
        [JsonPropertyName("total_cart_item")]
        public int TotalCartItem { get; set; }
    }
    public class PaymentRes
    {
        public string Url { get; set; }
    }
}
