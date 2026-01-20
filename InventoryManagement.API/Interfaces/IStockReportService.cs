using InventoryManagement.API.DTOs;

namespace InventoryManagement.API.Interfaces
{
    public interface IStockReportService
    {
        Task<PaginatedResponseDto<StockSummaryDto>> GetStockSummaryAsync(PaginationQueryDto query);
        Task<PaginatedResponseDto<StockSummaryDto>> GetLowStockAsync(
            PaginationQueryDto query,
            int threshold
        );
    }
}
