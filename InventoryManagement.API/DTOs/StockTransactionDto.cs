using InventoryManagement.API.Entities;

namespace InventoryManagement.API.DTOs
{
    public class StockTransactionDto
    {
        public int Id { get; set; }                 
        public int ProductId { get; set; }            

        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }           

        public TransactionTypeEnum TransactionType { get; set; } 

        public string? Notes { get; set; }          

        public DateTime CreatedAt { get; set; }
    }
}
