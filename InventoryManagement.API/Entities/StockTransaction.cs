namespace InventoryManagement.API.Entities
{
    public class StockTransaction
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }   

        public TransactionTypeEnum TransactionType { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
