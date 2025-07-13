using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commons.DTOs.Supplier
{
    public class TypeOfServiceRatingDto
    {
        public string Name { get; set; }
    }

    public class SuppliersRatingResDto
    {
        public string Id { get; set; }
        public double Rating { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Thumbnail { get; set; }

        [JsonPropertyName("type_service")]
        public List<TypeOfServiceRatingDto> TypeService { get; set; }
    }

}
