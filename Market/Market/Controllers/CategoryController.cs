using Market.Models;
using Microsoft.AspNetCore.Mvc;


namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        [HttpPost("addCategory")]
        public IActionResult AddCategory([FromQuery] string name)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Categories.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Add(new Category()
                        {
                            Name = name

                        });
                        context.SaveChanges();
                        return Ok();
                    }

                    return StatusCode(409);

                }
            }
            catch
            {
                return StatusCode(500);
            }
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
    }
}
