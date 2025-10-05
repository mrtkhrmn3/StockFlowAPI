namespace StockFlowAPI.BusinessLayer.DTOs
{
    public class UpdateProductDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Company { get; set; } = string.Empty;
    }
}
