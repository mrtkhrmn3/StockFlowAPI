using Microsoft.EntityFrameworkCore;

namespace StockFlowAPI.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Precision(18, 2)] // 18 hane toplam, 2 ondalık
        public decimal Price { get; set; }
        [Precision(18, 3)] // 18 hane toplam, 3 ondalık
        public decimal Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Company { get; set; } = string.Empty;

        // Foreign key
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
