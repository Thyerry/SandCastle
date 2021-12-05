using MongoDB.Bson.Serialization.Attributes;
using SandCastle_BackEnd.Misc;

namespace SandCastle_BackEnd.Entidades
{
    public class Jogo
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public List<string>? Jogadores { get; set; }
    }
}
