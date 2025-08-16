using System.Diagnostics.CodeAnalysis;

namespace RealEstate.Infrastructure.Mongo
{
    [ExcludeFromCodeCoverage]
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
        public string OwnersCollection { get; set; } = "Owners";


    }
}
