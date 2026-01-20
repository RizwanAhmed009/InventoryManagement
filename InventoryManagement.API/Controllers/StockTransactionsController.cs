using InventoryManagement.API.DTOs;
using InventoryManagement.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class StockTransactionsController : ControllerBase
{
    private readonly IStockService service;

    public StockTransactionsController(IStockService service)
    {
        this.service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var transactions = await service.GetAllTransactionsAsync();
        return Ok(transactions);
    }

    [HttpGet("product/{productId}")]
    public async Task<IActionResult> GetTransactionByProduct(int productId)
    {
        var transaction = await service.GetTransactionsByProductAsync(productId);
        if (transaction == null) return NotFound();
        return Ok(transaction);
    }

    [HttpPost("in")]
    public async Task<IActionResult> AddStock(
      [FromBody] StockInOutRequestDto dto)
    {
        var result = await service.AddStockAsync(dto.ProductId, dto.Quantity);

        if (result == null)
            return NotFound("Product not found");

        return Ok(result);
    }


    [HttpPost("out")]
    public async Task<IActionResult> RemoveStock(
    [FromBody] StockInOutRequestDto dto)
    {
        var result = await service.RemoveStockAsync(dto.ProductId, dto.Quantity);

        if (result == null)
            return NotFound("Product not found");

        return Ok(result);
    }

}
