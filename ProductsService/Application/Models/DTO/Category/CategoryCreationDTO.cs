using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Models.DTO.Category
{
    public class CategoryCreationDTO
    {
        [JsonPropertyName("name")]
        public string Name {  get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("parent_category")]
        public Guid? ParentCategoryId { get; set; }
    }
}
