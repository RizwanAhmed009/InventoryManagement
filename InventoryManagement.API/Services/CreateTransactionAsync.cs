using InventoryManagement.API.Data;
using InventoryManagement.API.DTOs;
using InventoryManagement.API.Entities;
using InventoryManagement.API.Exceptions;
using InventoryManagement.API.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StockTransactionService : IStockTransactionService
{
    private readonly AppDbContext context;

    public StockTransactionService(AppDbContext context)
    {
        this.context = context;
    }

    public async Task<StockTransactionDto> CreateTransactionAsync(
        StockTransactionCreateDto dto)
    {

        if (dto.Quantity <= 0)
            throw new BadRequestException("Quantity must be greater than zero");

        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId && p.IsActive);

        if (product == null)
            throw new NotFoundException("Product not found");

        if (dto.TransactionType == TransactionTypeEnum.OUT)
        {
            if (product.Quantity < dto.Quantity)
                throw new BadRequestException("Not enough stock available");

            product.Quantity -= dto.Quantity;
        }
        else
        {
            product.Quantity += dto.Quantity;
        }

        product.TotalPrice = product.Price * product.Quantity;

        var transaction = new StockTransaction
        {
            ProductId = product.Id,
            Quantity = dto.Quantity,
            TransactionType = dto.TransactionType,
            CreatedAt = DateTime.UtcNow
        };

        context.StockTransactions.Add(transaction);
        await context.SaveChangesAsync();

        return new StockTransactionDto
        {
            Id = transaction.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = transaction.Quantity,
            TransactionType = transaction.TransactionType,
            CreatedAt = transaction.CreatedAt!.Value
        };
    }
}
