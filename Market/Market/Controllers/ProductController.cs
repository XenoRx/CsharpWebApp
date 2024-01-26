using Market.Abstraction;
using Market.Models;
using Market.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace Market.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost("addProduct")]
        public IActionResult addProducts([FromBody] DTOProduct product)
        {
            var result = _productRepository.AddProduct(product);

            return Ok(result);
        }

        [HttpGet("getProduct")]
        public IActionResult GetProducts()
        {
            var products = _productRepository.GetProducts();
            return Ok(products);
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

        [HttpGet("GetProductsCSV")]
        public FileContentResult GetCSV()
        {
            var content = _productRepository.GetProductsCSV();

            return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "Products.csv");
        }

        [HttpGet("GetCacheCSVUrl")]
        public ActionResult<string> GetCacheCSVUrl()
        {
            var result = _productRepository.GetCacheStatCSV();

            if (result is not null)
            {
                var fileName = $"products{DateTime.Now.ToBinary()}.csv";

                System.IO.File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles", fileName), result);

                return "https://" + Request.Host.ToString() + "/static/" + fileName;
            }

            return StatusCode(500);
        }
    }
}