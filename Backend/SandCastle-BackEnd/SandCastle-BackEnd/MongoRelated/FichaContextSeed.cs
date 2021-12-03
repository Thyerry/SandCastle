using MongoDB.Driver;

namespace SandCastle_BackEnd.MongoRelated
{
    public class FichaContextSeed
    {
        public static void SeedData(IMongoCollection<Ficha> fichaCollection)
        {
            bool ficha = fichaCollection.Find(p => true).Any();
            if (!ficha)
            {
                fichaCollection.InsertOne(new Ficha()
                {
                    Nome = "Thyerry"
                });
            }
        }
    }
}