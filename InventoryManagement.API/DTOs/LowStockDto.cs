namespace InventoryManagement.API.DTOs
{
    public class LowStockDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int AvailableQuantity { get; set; }
        public int Threshold { get; set; }
    }
}
