using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services.DTOs
{
    public class ServiceDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("thumbnail")]
        public string ThumbnailUrl { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("category_id")]
        public Guid? CategoryId { get; set; } = null;

        [JsonPropertyName("supplier")]
        public ProductDetailSupplier? Supplier { get; set; }

        [JsonPropertyName("parent_id")]
        public Guid? ParentServiceId { get; set; } = null;

        [JsonPropertyName("images")]
        public List<ProductUploadedImage> ServiceImages { get; set; } = new();

        [JsonPropertyName("packages")]
        public List<ProductRentalOption> RentalOptions { get; set; } = new();
    }
    public class ProductRentalOption
    {
        [JsonPropertyName("package_name")]
        public string PackageName { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        [JsonPropertyName("overtime_price")]
        public decimal? OvertimePrice { get; set; }

        [JsonPropertyName("minimum_hours")]
        public int MinimumHours { get; set; }
    }

    public class ProductUploadedImage
    {
        [JsonPropertyName("id")]
        public Guid ImageId { get; set; }
        [JsonPropertyName("order")]
        public int Order { get; set; }
        [JsonPropertyName("url")]
        public string ImageUrl { get; set; }
    }
    public class ProductDetailSupplier
    {
        [JsonPropertyName("name")]
        public string SupplierName { get; set; }
        [JsonPropertyName("supplier_id")]
        public Guid? SupplierId { get; set; }

        [JsonPropertyName("is_active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("location")]
        public string SupplierLocation { get; set; }

        [JsonPropertyName("avatar")]
        public string AvatarUrl { get; set; }
    }
    public class ProductDetailCustomer
    {
        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }
        [JsonPropertyName("customer_phone")]
        public string? CustomerPhone{ get; set; }
    }
}