namespace InventoryManagement.API.DTOs
{
    public class StockSummaryDto  
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
