namespace RealEstate.Application.Owers.DTOs
{
    public class PropertyTraceDto
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}
