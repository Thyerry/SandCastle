using MongoDB.Driver;

namespace SandCastle_BackEnd.MongoRelated
{
    public class FichaContext : IFichaContext
    {
        public FichaContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Fichas = database.GetCollection<Ficha>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
        }
        public IMongoCollection<Ficha> Fichas { get; }
    }
}
