using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace SnappFoodApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(NewProductReq product)
    {
        try
        {
            await _productService.AddProductAsync(product);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/increase-inventory")]
    public async Task<IActionResult> IncreaseInventory(long id, long quantity)
    {
        try
        {
            await _productService.IncreaseInventoryAsync(id, quantity);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(long id)
    {
        try
        {
            var product = await _productService.GetProductAsync(id);
            return Ok(product);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("buy/{productId}")]
    public async Task<IActionResult> BuyProduct(int productId, int userId)
    {
        try
        {
            await _productService.BuyProductAsync(productId, userId);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}