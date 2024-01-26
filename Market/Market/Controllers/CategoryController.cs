using Market.Abstraction;
using Market.Models;
using Market.Models.DTO;
using Microsoft.AspNetCore.Mvc;


namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromBody] DTOCategory category)
        {
            var result = _categoryRepository.AddCategory(category);
            return Ok(result);
        }

        [HttpDelete("deleteCategory")]
        public IActionResult DeleteCategory([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Categories.Any(x => x.CategoryId == id))
                    {
                        return NotFound();
                    }

                    Category product = context.Categories.FirstOrDefault(x => x.CategoryId == id)!;
                    context.Categories.Remove(product);
                    context.SaveChanges();

                    return Ok();

                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpGet("getCategorys")]
        public IActionResult GetCategorys()
        {
            var result = _categoryRepository.GetCategories();
            return Ok(result);
        }

        [HttpGet("GetCategorysCSV")]
        public FileContentResult GetCSV()
        {
            var content = _categoryRepository.GetCategoriesCSV();

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "Categorys.csv");
        }

        [HttpGet("GetCacheCSV")]
        public ActionResult<string> GetCacheCSV()
        {
            string result = _categoryRepository.GetСacheStatCSV();

            if (result is not null)
            {
                var fileName = $"categorys{DateTime.Now.ToBinary()}.csv";

                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), result);

                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }
            return StatusCode(500);
        }
    }
}
