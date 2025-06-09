using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class CartRespondeDTO
    {
        public string CartId { get; set; } = string.Empty;
        public List<CartItemResponse> Content { get; set; } = new();
        
    }
    public class CartItemResponse
    {
        public Guid ServiceId { get; set; } = Guid.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}
