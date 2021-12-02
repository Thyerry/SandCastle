using MongoDB.Driver;

namespace SandCastle_BackEnd.MongoRelated
{
    public interface IFichaContext
    {
        IMongoCollection<Ficha> Fichas { get; }
    }
}
