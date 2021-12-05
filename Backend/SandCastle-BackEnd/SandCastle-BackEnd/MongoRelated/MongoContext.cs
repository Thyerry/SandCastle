using MongoDB.Driver;
using SandCastle_BackEnd.Entidades;

namespace SandCastle_BackEnd.MongoRelated
{
    public class MongoContext : IMongoContext
    {
        public MongoContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
            Fichas = database.GetCollection<Ficha>("Ficha");
            Jogos = database.GetCollection<Jogo>("Jogo");
            Jogadores = database.GetCollection<Jogador>("Jogadores");
        }
        public IMongoCollection<Ficha> Fichas { get; }
        public IMongoCollection<Jogo> Jogos { get; }
        public IMongoCollection<Jogador> Jogadores { get; }
    }
}
