using Market.Models.DTO;

namespace Market.Abstraction
{
    public interface ICategoryRepository
    {
        public int AddCategory(DTOCategory category);
        public string GetCategoriesCSV();
        public string GetСacheStatCSV();

        public IEnumerable<DTOCategory> GetCategories();
    }
}
