using Ecommerce.Models;
using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        private readonly ILogger<ProductController> _logger;
        /*        private readonly IMapper _mapper;*/

        public ProductController(IConfiguration config, ILogger<ProductController> logger)
        {
            _config = config;
            _ef = new DataContextEf(_config);
            _logger = logger;
        }


        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems()
        {
            try
            {
                IEnumerable<Product> products = await _ef.products.ToListAsync<Product>();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSingleItem/{id}")]
        public async Task<IActionResult> GetSingleItem(int id)
        {
            try
            {
                Product? product = await _ef.products.SingleOrDefaultAsync(k => k.ProductId == id);
                return Ok(product);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("PostItems")] //post new item
        public async Task<IActionResult> PostItems(ProductDto productDto, int Id)
        {
            try
            {
                Product productDb = new Product()
                {
                    ProductId = Id,
                    Name = productDto.Name,
                    Price = productDto.Price,
                    StockQuantity = productDto.StockQuantity,


                };
                await _ef.products.AddAsync(productDb);
                await _ef.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("UpdateItems")]
        public async Task<IActionResult> UpdateItems(ProductDto productDto, int Id)
        {
            try
            {
                Product? productDb = await _ef.products.Where(k => k.ProductId == Id).FirstOrDefaultAsync(); //select the product
                productDb.Name = productDto.Name;
                productDb.Price = productDto.Price;
                productDb.StockQuantity = productDto.StockQuantity;
                await _ef.SaveChangesAsync();
                return Ok(productDb);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteItems")]
        public async Task<IActionResult> DeleteItems(int Id)
        {
            try
            {
                Product? product = await _ef.products.Where(k => k.ProductId == Id).FirstOrDefaultAsync();
                _ef.products.Remove(product);
                await _ef.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
