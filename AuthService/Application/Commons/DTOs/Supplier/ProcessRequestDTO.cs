using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class ProcessRequestDTO
    {
        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }
        [JsonPropertyName("inspector_id")]
        public Guid? InspectorId { get; set; }
        public bool? IsAccept
        {
            get; set;
        }
    }
}