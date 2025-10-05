using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockFlowAPI.BusinessLayer.DTOs;
using StockFlowAPI.BusinessLayer.Interfaces;

namespace StockFlowAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        private Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst("UserId")!.Value);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("my-products")]
        public async Task<IActionResult> GetMyProducts()
        {
            var products = await _productService.GetMyProductsAsync();
            return Ok(products);
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy([FromBody] CreateProductDTO dto)
        {
            var result = await _productService.CreateAsync(dto);
            if (!result) return BadRequest("Ürün eklenemedi.");
            return Ok("Ürün başarıyla eklendi.");
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto)
        {
            var result = await _productService.UpdateAsync(dto);
            if (!result) return BadRequest("Ürün güncellenemedi.");
            return Ok("Ürün başarıyla güncellendi.");
        }

        [HttpDelete("sell/{id}")]
        public async Task<IActionResult> Sell(Guid id)
        {
            var result = await _productService.DeleteAsync(id);
            if (!result) return BadRequest("Ürün silinemedi.");
            return Ok("Ürün başarıyla satıldı.");
        }
    }
}