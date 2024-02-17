using Ecommerce.Models;
using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce2.Controllers
{
    public class ProductCategoryController : ControllerBase
    {
        private readonly DataContextEf _ef;
        private readonly IConfiguration _config;
        public ProductCategoryController(IConfiguration configuration)
        {
            _config = configuration;
            _ef = new DataContextEf(configuration);
        }


        [HttpGet("GetProductCategory")]
        public IActionResult GetProductCategory()
        {
            IEnumerable<ProductCategory> productsDb = _ef.productCategories.ToList<ProductCategory>();
            return Ok(productsDb);
        }

        [HttpPost("AddProductCategory")]
        public IActionResult AddProductCategory(ProductCategoryDto productCategoryDto)
        {
            Product? productDb = _ef.products.Where(p => p.ProductId == productCategoryDto.ProductId).FirstOrDefault();
            Category? categoryDb = _ef.categories.Where(p => p.CategoryId == productCategoryDto.CategoryId).FirstOrDefault();

            /*            if (productDb != null || categoryDb != null)
                        {
                            throw new Exception($"The product {productDb} or the category {categoryDb} dosent`t exist!");
                        }*/

            ProductCategory productCategory = new ProductCategory()
            {
                ProductId = productCategoryDto.ProductId,
                Product = productDb,
                CategoryId = productCategoryDto.CategoryId,
                Category = categoryDb
            };

            _ef.productCategories.Add(productCategory);
            _ef.SaveChanges();

            return Ok(productCategory);
        }


        [HttpPut("UpdateProductCategory")]
        public IActionResult UpdateProductCategory(ProductCategoryDto productCategoryDto, int Id)
        {
            ProductCategory? productCategoryDb = _ef.productCategories.FirstOrDefault(p => p.ProductId == productCategoryDto.ProductId && p.CategoryId == productCategoryDto.CategoryId);

            if (productCategoryDb == null)
            {
                throw new Exception($"The product category with ProductId {productCategoryDto.ProductId} and CategoryId {productCategoryDto.CategoryId} doesn't exist!");
            }

            productCategoryDb.ProductId = productCategoryDto.ProductId;
            /*                productCategoryDb.Product = productDb,*/
            productCategoryDb.CategoryId = productCategoryDto.CategoryId;
            /*                productCategoryDb.Category = categoryDb*/

            _ef.productCategories.Update(productCategoryDb);
            _ef.SaveChanges();

            return Ok(productCategoryDb);
        }
        [HttpDelete("DeleteProductCategory")]
        public IActionResult DeleteProductCategory(int id)
        {
            ProductCategory? productCategoryDb = _ef.productCategories.Where(p => p.ProductCategoryId == id).FirstOrDefault();

            _ef.productCategories.Remove(productCategoryDb);
            _ef.SaveChanges();

            return Ok(productCategoryDb);
        }
    }
}
