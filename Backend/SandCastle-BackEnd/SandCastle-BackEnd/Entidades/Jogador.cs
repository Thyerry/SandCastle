using MongoDB.Bson.Serialization.Attributes;
using SandCastle_BackEnd.Misc;

namespace SandCastle_BackEnd.Entidades
{
    public class Jogador
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Nome { get; set; }
        public TipoJogador Tipo{ get; set; }
    }
}