namespace SandCastle_BackEnd.MongoRelated.Repository
{
    public interface IFichaRepository
    {
        Task<IEnumerable<Ficha>> GetFichas();
        Task<Ficha> GetById(int id);
        Task<IEnumerable<Ficha>> GetFichaByName(string name);
        Task CreateFicha(Ficha ficha);
        Task<bool> UpdateFicha(Ficha ficha);
        Task<bool> DeleteById(int id);
    }
}
