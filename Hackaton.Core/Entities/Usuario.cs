using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public abstract class Usuario : EntityBase
    {
        [BsonElement("nome")]
        public required string Nome { get; set; }

        [BsonElement("email")]
        public required string Email { get; set; }

        [BsonElement("senha")]
        public required string Senha { get; set; }
    }
}
