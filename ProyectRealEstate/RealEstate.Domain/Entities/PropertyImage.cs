using System.Diagnostics.CodeAnalysis;

namespace RealEstate.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class PropertyImage
    {
        public string File { get; set; } = default!;
        public bool Enabled { get; set; } = true;
    }
}
