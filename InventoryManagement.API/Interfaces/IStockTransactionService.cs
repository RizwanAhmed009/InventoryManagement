using InventoryManagement.API.DTOs;

namespace InventoryManagement.API.Interfaces
{
    public interface IStockTransactionService
    {
        Task<StockTransactionDto> CreateTransactionAsync(
            StockTransactionCreateDto dto);
    }

}
