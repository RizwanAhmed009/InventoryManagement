using InventoryManagement.API.Data;

using InventoryManagement.API.DTOs;

using InventoryManagement.API.Entities;

using InventoryManagement.API.Interfaces;

using Microsoft.EntityFrameworkCore;



namespace InventoryManagement.API.Services

{

    public class StockService : IStockService

    {

        private readonly AppDbContext context;



        public StockService(AppDbContext context)

        {

            this.context = context;

        }



        // ================= ADD STOCK =================

        public async Task<StockInOutRequestDto?> AddStockAsync(int productId, int quantity)
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null) return null;

            var transaction = new StockTransaction
            {
                ProductId = productId,
                Quantity = quantity,              
                TransactionType = TransactionTypeEnum.IN,
                  CreatedAt = DateTime.UtcNow 
            };

            context.StockTransactions.Add(transaction);

            product.Quantity += quantity;
            product.TotalPrice = product.Price * product.Quantity;

            await context.SaveChangesAsync();

            return new StockInOutRequestDto
            {
                ProductId = productId,
                Quantity = quantity
            };
        }




        // ================= REMOVE STOCK =================
        public async Task<StockInOutRequestDto?> RemoveStockAsync(int productId, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than 0");

            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null)
                return null;

            if (product.Quantity < quantity)
                throw new Exception("Not enough stock");

            var transaction = new StockTransaction
            {
                ProductId = productId,
                Quantity = quantity, 
                TransactionType = TransactionTypeEnum.OUT,
                CreatedAt = DateTime.UtcNow
            };

            context.StockTransactions.Add(transaction);

            product.Quantity -= quantity;
            product.TotalPrice = product.Price * product.Quantity;

            await context.SaveChangesAsync();

            return new StockInOutRequestDto
            {
                ProductId = productId,
                Quantity = quantity
            };
        }

        // ================= GET ALL =================

        public async Task<IEnumerable<StockTransactionDto>> GetAllTransactionsAsync()

        {

            var transactions = await context.StockTransactions

                .Include(t => t.Product)

                .ToListAsync();



            return transactions.Select(t => MapToDto(t, t.Product));

        }



        // ================= GET BY PRODUCT =================

        public async Task<IEnumerable<StockTransactionDto>> GetTransactionsByProductAsync(int productId)

        {

            var transactions = await context.StockTransactions

                .Include(t => t.Product)

                .Where(t => t.ProductId == productId)

                .ToListAsync();



            return transactions.Select(t => MapToDto(t, t.Product));

        }



        // ================= PRIVATE MAPPER =================

        private static StockTransactionDto MapToDto(StockTransaction t, Product product)
        {
            return new StockTransactionDto
            {
                Id = t.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = t.Quantity,
                TransactionType = t.TransactionType,
                CreatedAt = t.CreatedAt ?? DateTime.UtcNow

            };
        }


    }

}