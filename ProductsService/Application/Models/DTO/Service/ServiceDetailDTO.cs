using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Models.DTO.Service
{
    public class ServiceDetailDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("category_id")]
        public Guid CategoryId { get; set; }

        [JsonPropertyName("supplier_id")]
        public Guid SupplierId { get; set; }
        [JsonPropertyName("supplier")]
        public Supplier? Supplier { get; set; }

        [JsonPropertyName("supplier_name")]
        public string SupplierName { get; set; } = string.Empty;

        [JsonPropertyName("parent_id")]
        public Guid ParentServiceId { get; set; }

        [JsonPropertyName("images")]
        public List<string> ServiceImageUrls { get; set; } = new();

        [JsonPropertyName("packages")]
        public List<RentalOptionDto> RentalOptions { get; set; } = new();

    }
    public class Supplier
    {

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        [JsonPropertyName("total_service")]
        public int TotalService { get; set; }
    }
    public class RentalOptionDto
    {
        [JsonPropertyName("package_name")]
        public PackageType PackageName { get; set; }
        [JsonPropertyName("minimum_hours")]
        public int MinimumHours { get; set; }
        [JsonPropertyName("hourly_surcharge")]
        public decimal? HourlySurcharge { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
