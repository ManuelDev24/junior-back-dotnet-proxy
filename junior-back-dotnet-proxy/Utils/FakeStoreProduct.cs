namespace junior_back_dotnet_proxy.Utils
{
    // Represents a product retrieved from the FakeStore API
    public class FakeStoreProduct
    {
        public int Id { get; set; } // Unique identifier of the product
        public string Title { get; set; } = string.Empty; // Product title or name
        public decimal Price { get; set; } // Product price
        public string Description { get; set; } = string.Empty; // Product description
        public string Category { get; set; } = string.Empty; // Category the product belongs to
        public string Image { get; set; } = string.Empty; // URL of the product image
        public Rating Rating { get; set; } = new(); // Product rating (rate and count)
    }

    // Represents the rating information of a product
    public class Rating
    {
        public decimal Rate { get; set; } // Average rating (e.g., 4.5)
        public int Count { get; set; } // Number of reviews or ratings
    }
}
