namespace Market.Models
{
    public class Storage
    {
        public virtual List<Product>? Products { get; set; } = null!;

        public int StorageId { get; set; }

        public string? Name { get; set; }

        public int Quantity { get; set; }

    }
}
