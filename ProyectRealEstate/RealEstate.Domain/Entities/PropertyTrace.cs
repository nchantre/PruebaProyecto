using System.Diagnostics.CodeAnalysis;

namespace RealEstate.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class PropertyTrace
    {
        public DateTime DateSale { get; set; }
        public string Name { get; set; } = default!;
        public decimal Value { get; set; }
        public decimal Tax { get; set; }
    }
}
