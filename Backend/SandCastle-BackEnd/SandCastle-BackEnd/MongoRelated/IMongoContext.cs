using MongoDB.Driver;
using SandCastle_BackEnd.Entidades;

namespace SandCastle_BackEnd.MongoRelated
{
    public interface IMongoContext
    {
        IMongoCollection<Ficha> Fichas { get; }
        IMongoCollection<Jogo> Jogos { get; }
        IMongoCollection<Jogador> Jogadores { get; }
    }
}
