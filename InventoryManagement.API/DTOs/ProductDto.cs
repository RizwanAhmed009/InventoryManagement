namespace InventoryManagement.API.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }             
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }     
        public string? CategoryName { get; set; }  
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
