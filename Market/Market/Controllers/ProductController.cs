using Market.Models;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpGet("getProduct")]
        public IActionResult GetProducts()
        {
            try
            {
                using (var context = new ProductContext())
                {
                    var products = context.Products.Select(x => new Product()
                    {
                        ProductId = x.ProductId,
                        Name = x.Name,
                        Description = x.Description,
                    });
                    return Ok(products);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpPost("addProduct")]
        public IActionResult addProducts([FromQuery] string name, string description, int categoryId, int cost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.Name.ToLower().Equals(name)))
                    {
                        context.Add(new Product()
                        {
                            Name = name,
                            Description = description,
                            Cost = cost,
                            CategoryID = categoryId
                        });
                        context.SaveChanges();
                        return Ok();
                    }
                    return StatusCode(409);

                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("deleteProduct")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.ProductId == id))
                    {
                        return NotFound();
                    }

                    Product product = context.Products.FirstOrDefault(x => x.ProductId == id)!;
                    context.Products.Remove(product);
                    context.SaveChanges();

                    return Ok();

                }
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpPut("addProductCost")]
        public IActionResult AddProductPrice([FromQuery] int id, int cost)
        {
            try
            {
                using (var context = new ProductContext())
                {
                    if (!context.Products.Any(x => x.ProductId == id))
                    {

                        return NotFound();
                    }

                    Product product = context.Products.FirstOrDefault(x => x.ProductId == id)!;
                    product.Cost = cost;
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

