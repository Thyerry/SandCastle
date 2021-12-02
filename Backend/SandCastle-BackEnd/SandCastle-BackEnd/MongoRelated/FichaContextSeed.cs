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
                fichaCollection.InsertMany(GetFichas());
            }
        }

        private static IEnumerable<Ficha> GetFichas()
        {
            return new List<Ficha>() 
            {
                new Ficha()
            };
        }
    }
}