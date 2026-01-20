using InventoryManagement.API.Entities;

public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public decimal TotalPrice { get; set; }

    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<StockTransaction> StockTransactions { get; set; }
        = new List<StockTransaction>();
}
