using MongoDB.Bson.Serialization.Attributes;

namespace SandCastle_BackEnd.Entidades
{
    public class Ficha
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? IdJogador { get; set; }
        public string? IdJogo { get; set; }
        [BsonElement("Nome")]
        public string? Nome { get; set; }
        public string? Classe { get; set; }
        public string? Especificidades { get; set; }
        public int Força { get; set; }
        public int Destreza { get; set; }
        public int Inteligencia { get; set; }
        public int Vigor { get; set; }
    }
}
