using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace RealEstate.Domain.Entities
{
    [ExcludeFromCodeCoverage]
    public class Ownert
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? IdOwner { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Photo { get; set; } = default!;
        public DateTime Birthday { get; set; }
        public List<Property> Properties { get; set; } = new();
    }
}
