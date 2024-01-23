namespace Market.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; } = null!;

        public int Cost { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; } = new Category();

        public virtual List<Storage> Stores { get; set; } = null!;

    }
}