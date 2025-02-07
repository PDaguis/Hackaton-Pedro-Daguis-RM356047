using Hackaton.Core.Enumerators;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public class Paciente : Usuario
    {
        [BsonElement("cpf")]
        public required string Cpf { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("genero")]
        public EGenero? Genero { get; set; }
    }
}
