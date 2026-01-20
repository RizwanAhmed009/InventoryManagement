using InventoryManagement.API.Data;
using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.API.Services
{
    public class StockReportService : IStockReportService
    {
        private readonly AppDbContext _context;

        public StockReportService(AppDbContext context)
        {
            _context = context;
        }

        // ===========================
        // 1️⃣ Stock Summary Report
        // ===========================
        public async Task<PaginatedResponseDto<StockSummaryDto>> GetStockSummaryAsync(PaginationQueryDto query)
        {
            var queryable = _context.StockTransactions
                .Include(t => t.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductName))
                queryable = queryable.Where(t => t.Product.Name.Contains(query.ProductName));

            if (query.FromDate.HasValue)
                queryable = queryable.Where(t => t.CreatedAt >= query.FromDate.Value);

            if (query.ToDate.HasValue)
                queryable = queryable.Where(t => t.CreatedAt <= query.ToDate.Value);

            // ✅ Aggregate and project to DTO first
            var reportList = await queryable
                .GroupBy(t => new
                {
                    t.ProductId,
                    ProductName = t.Product.Name,
                    Category = t.Product.Category,
                    Price = t.Product.Price,
                    IsActive = t.Product.IsActive
                })
                .Select(g => new StockSummaryDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Category = g.Key.Category.ToString(),
                    UnitPrice = g.Key.Price,
                    Status = g.Key.IsActive ? "Active" : "Inactive",
                    TotalIn = g.Where(x => x.TransactionType == TransactionTypeEnum.IN).Sum(x => x.Quantity),
                    TotalOut = g.Where(x => x.TransactionType == TransactionTypeEnum.OUT).Sum(x => x.Quantity),
                    AvailableQuantity = g.Where(x => x.TransactionType == TransactionTypeEnum.IN).Sum(x => x.Quantity)
                                      - g.Where(x => x.TransactionType == TransactionTypeEnum.OUT).Sum(x => x.Quantity),
                    TotalAmount = (g.Where(x => x.TransactionType == TransactionTypeEnum.IN).Sum(x => x.Quantity)
                                  - g.Where(x => x.TransactionType == TransactionTypeEnum.OUT).Sum(x => x.Quantity))
                                  * g.Key.Price
                })
                .ToListAsync(); // ✅ materialize first

            // Pagination
            var totalRecords = reportList.Count;
            var pagedData = reportList
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new PaginatedResponseDto<StockSummaryDto>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords,
                Data = pagedData
            };
        }

        // ===========================
        // 2️⃣ Low Stock Report
        // ===========================
        public async Task<PaginatedResponseDto<StockSummaryDto>> GetLowStockAsync(PaginationQueryDto query, int threshold)
        {
            var lowStockList = await _context.StockTransactions
                .Include(t => t.Product)
                .GroupBy(t => new
                {
                    t.ProductId,
                    ProductName = t.Product.Name,
                    Category = t.Product.Category,
                    Price = t.Product.Price,
                    IsActive = t.Product.IsActive
                })
                .Select(g => new StockSummaryDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    Category = g.Key.Category.ToString(),
                    UnitPrice = g.Key.Price,
                    Status = g.Key.IsActive ? "Active" : "Inactive",
                    AvailableQuantity = g.Where(x => x.TransactionType == TransactionTypeEnum.IN).Sum(x => x.Quantity)
                                      - g.Where(x => x.TransactionType == TransactionTypeEnum.OUT).Sum(x => x.Quantity)
                })
                .Where(x => x.AvailableQuantity <= threshold)
                .ToListAsync(); // ✅ materialize first

            var totalRecords = lowStockList.Count;
            var pagedData = lowStockList
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new PaginatedResponseDto<StockSummaryDto>
            {
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalRecords = totalRecords,
                Data = pagedData
            };
        }
    }
}
