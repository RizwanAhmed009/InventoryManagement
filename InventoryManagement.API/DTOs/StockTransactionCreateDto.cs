using InventoryManagement.API.Entities;

namespace InventoryManagement.API.DTOs
{
    public class StockTransactionCreateDto
    {
        public int ProductId { get; set; }           
        public int Quantity { get; set; }           
        public TransactionTypeEnum TransactionType { get; set; } 
        public string? Notes { get; set; }
    }
}
