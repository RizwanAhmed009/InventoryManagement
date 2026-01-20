using InventoryManagement.API.DTOs;

namespace InventoryManagement.API.Interfaces
{
    public interface IStockService
    {
        Task<StockInOutRequestDto?> AddStockAsync(int productId, int quantity);
        Task<StockInOutRequestDto?> RemoveStockAsync(int productId, int quantity);

        Task<IEnumerable<StockTransactionDto>> GetAllTransactionsAsync();
        Task<IEnumerable<StockTransactionDto>> GetTransactionsByProductAsync(int productId);
    }
}
