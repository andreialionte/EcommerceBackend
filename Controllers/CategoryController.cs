using Ecommerce2.Data;
using Ecommerce2.Dtos;
using Ecommerce2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce2.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DataContextEf _ef;
        public CategoryController(IConfiguration config)
        {
            _config = config;
            _ef = new DataContextEf(config);
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory()
        {
            IEnumerable<Category> categoryDb = _ef.categories.ToList();
            return Ok(categoryDb);
        }

        [HttpPost("AddCategory")]
        public IActionResult AddCategory(CategoryDto categoryDto, int Id)
        {
            Category? categoryDb = new Category()
            {
                Name = categoryDto.Name,
                CategoryId = Id
            };
            _ef.categories.Add(categoryDb);
            _ef.SaveChanges();
            return Ok(categoryDb);
        }

        [HttpPut("UpdateCategory")]
        public IActionResult UpdateCategory(int Id, CategoryDto categoryDto)
        {
            Category? categoryDb = _ef.categories.Where(k => k.CategoryId == Id).FirstOrDefault();

            if (categoryDb == null)
            {
                return NotFound(categoryDb);
            }

            categoryDb.Name = categoryDto.Name;
            _ef.Update(categoryDb);
            _ef.SaveChanges();
            return Ok(categoryDb);
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int Id)
        {
            Category? categoryDb = _ef.categories.Where(k => k.CategoryId == Id).FirstOrDefault();
            _ef.categories.Remove(categoryDb);

            return Ok(categoryDb);
        }
    }
}
