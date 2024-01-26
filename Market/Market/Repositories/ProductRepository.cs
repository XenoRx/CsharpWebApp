using AutoMapper;
using Market.Abstraction;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Market.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public ProductRepository(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }

        public int AddProduct(DTOProduct product)
        {
            using (var context = new ProductContext())
            {
                var entityProduct = context.Products.FirstOrDefault(x => x.Name.ToLower() == product.Name.ToLower());
                if (entityProduct is null)
                {
                    entityProduct = _mapper.Map<Product>(product);
                    context.Products.Add(entityProduct);
                    context.SaveChanges();
                    _cache.Remove("products");
                }
                return entityProduct.ProductId;
            }
        }

        public IEnumerable<DTOProduct> GetProducts()
        {
            using (var context = new ProductContext())
            {
                if (_cache.TryGetValue("products", out List<DTOProduct> products))
                {
                    return products;
                }

                _cache.Set("products", products, TimeSpan.FromMinutes(30));

                var stat = _cache.GetCurrentStatistics().ToString();
                File.WriteAllText(Path.Combine(Environment.CurrentDirectory, "productcache.txt"), stat);

                products = context.Products.Select(x => _mapper.Map<DTOProduct>(x)).ToList();

                return products;
            }
        }
        public string GetProductsCSV()
        {
            var sb = new StringBuilder();
            var products = GetProducts();

            foreach (var product in products)
            {
                sb.AppendLine($"{product.ProductId},{product.Name}, {product.Description}");
            }

            return sb.ToString();
        }

        public string GetCacheStatCSV()
        {
            var curCache = _cache.GetCurrentStatistics();
            var sb = new StringBuilder();
            sb.AppendLine($"CurrentEntryCount, {curCache.CurrentEntryCount.ToString()}")
              .AppendLine($"CurrentEstimatedSize, {curCache.CurrentEstimatedSize.ToString()}")
              .AppendLine($"TotalHits, {curCache.TotalHits.ToString()}")
              .AppendLine($"TotalMisses, {curCache.TotalMisses.ToString()}");
            return sb.ToString();
        }
    }
}
