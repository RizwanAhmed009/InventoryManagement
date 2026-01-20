using InventoryManagement.API.DTOs;
using InventoryManagement.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IStockReportService service;

        public ReportsController(IStockReportService service)
        {
            this.service = service;
        }
        [HttpGet("stock")]
        public async Task<IActionResult> GetReport([FromQuery] PaginationQueryDto query)
        {
            var report = await service.GetStockSummaryAsync(query);
            return Ok(report);
        }

        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock(
    [FromQuery] PaginationQueryDto query,
    [FromQuery] int threshold = 5)
        {
            var result = await service.GetLowStockAsync(query, threshold);
            return Ok(result);
        }


    }
}
