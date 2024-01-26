using AutoMapper;
using Market.Abstraction;
using Market.Models;
using Market.Models.DTO;
using Microsoft.Extensions.Caching.Memory;
using System.Text;

namespace Market.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        public CategoryRepository(IMapper mapper, IMemoryCache cache)
        {
            _mapper = mapper;
            _cache = cache;
        }

        public int AddCategory(DTOCategory category)
        {
            using var context = new ProductContext();

            var entityCategory = context.Categories.FirstOrDefault(x => x.Name.ToLower() == category.Name.ToLower());
            if (entityCategory == null)
            {
                entityCategory = _mapper.Map<Category>(category);
                context.Categories.Add(entityCategory);
                context.SaveChanges();
                _cache.Remove("categories");
            }
            return entityCategory.CategoryId;
        }

        public IEnumerable<DTOCategory> GetCategories()
        {
            using (var context = new ProductContext())
            {
                if (_cache.TryGetValue("products", out List<DTOCategory>? categories))
                {
                    return categories;
                }
                _cache.Set("products", categories, TimeSpan.FromMinutes(30));
                var getCategorys = context.Categories.Select(x => _mapper.Map<DTOCategory>(x)).ToList();

                return getCategorys;
            }
        }

        public string GetCategoriesCSV()
        {
            var sb = new StringBuilder();
            var categories = GetCategories();

            foreach (var category in categories)
            {
                sb.AppendLine($"{category.CategoryId},{category.Name},{category.Description}");
            }

            return sb.ToString();
        }

        public string GetСacheStatCSV()
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
