namespace RealEstate.Infrastructure.Mongo
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
        public string OwnersCollection { get; set; } = "Owners";

     
    }
}
