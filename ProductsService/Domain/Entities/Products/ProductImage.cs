namespace Domain.Entities.Products
{
    public class ProductImage
    {
        public Guid Id { get; set; }

        public int Order { get; set; }

        public string ImageUrl { get; set; }

        public string AlternativeText { get; set; }

        public Guid ProductId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }
    }
}
