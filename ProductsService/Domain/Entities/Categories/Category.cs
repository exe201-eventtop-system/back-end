using Domain.Entities.Products;


namespace Domain.Entities.Categories
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid? ParentCategoryId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime LastModifiedAt { get; set; }

        public virtual Category? ParentCategoriesNavigation { get; set; }

        public virtual List<Category> ChildCategoriesNavigation { get; set; }

        public virtual List<Product> ServicesNavigation { get; set; }
    }
}
