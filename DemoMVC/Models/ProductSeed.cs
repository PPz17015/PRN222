namespace DemoMVC.Models
{
    public class ProductSeed
    {
        public static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High performance laptop", Category = "Electronics" },
                new Product { Id = 2, Name = "Smartphone", Price = 499.99m, Description = "Latest model smartphone", Category = "Electronics" },
                new Product { Id = 3, Name = "Headphones", Price = 199.99m, Description = "Noise-cancelling headphones", Category = "Accessories" },
                new Product { Id = 4, Name = "Smartwatch", Price = 299.99m, Description = "Feature-rich smartwatch", Category = "Wearables" },
                new Product { Id = 5, Name = "Tablet", Price = 399.99m, Description = "Portable tablet with high resolution display", Category = "Electronics" }
            };
        }
    }
}
