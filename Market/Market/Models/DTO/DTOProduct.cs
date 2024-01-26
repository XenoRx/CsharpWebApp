namespace Market.Models.DTO
{
    public class DTOProduct
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; } = null!;
    }
}
