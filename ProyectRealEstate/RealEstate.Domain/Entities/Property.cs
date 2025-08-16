using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace RealEstate.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Property
    {

        [BsonId]

        public string Id_Property { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = default!;
        public int Year { get; set; }
        public List<PropertyImage> Images { get; set; } = new();
        public List<PropertyTrace> Traces { get; set; } = new();
    }
}
