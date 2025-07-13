using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.DTOs.Supplier
{
    public class SupplierRatingDto
    {
        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }
        [JsonPropertyName("average_rating")]
        public double AverageRating { get; set; }
    }
}
