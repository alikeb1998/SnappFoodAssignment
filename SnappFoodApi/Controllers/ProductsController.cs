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

    [HttpPost(nameof(CacheProducts))]
    public async Task<IActionResult> CacheProducts()
    {
        var res = await _productService.CacheProducts();
        if (!res)
        {
            return BadRequest(res);
        }

        return Ok(res);
    }

    [HttpPost(nameof(AddProduct))]
    public async Task<IActionResult> AddProduct(NewProductReq product)
    {
       var res = await _productService.AddProductAsync(product);
       if (!res)
       {
           return BadRequest(res);
       }

       return Ok(res);
    }

    [HttpPut("IncreaseInventory/{id}")]
    public async Task<IActionResult> IncreaseInventory(long id, long quantity)
    {
        var res = await _productService.IncreaseInventoryAsync(id, quantity);
        if (!res)
        {
            return BadRequest(res);
        }

        return Ok(res);
    }

    [HttpGet("Product/{id}")]
    public async Task<IActionResult> GetProduct(long id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product is null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpPost("buy/{productId}")]
    public async Task<IActionResult> BuyProduct(long productId, long userId)
    {
        var result = await _productService.BuyProductAsync(productId, userId);
        if (!result)
        {
            return BadRequest(result);
        }
        return Ok(result);
    }
}