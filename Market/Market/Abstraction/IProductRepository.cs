using Market.Models.DTO;
namespace Market.Abstraction
{
    public interface IProductRepository
    {
        public int AddProduct(DTOProduct product);
        public string GetProductsCSV();
        public string GetCacheStatCSV();

        public IEnumerable<DTOProduct> GetProducts();
    }
}
