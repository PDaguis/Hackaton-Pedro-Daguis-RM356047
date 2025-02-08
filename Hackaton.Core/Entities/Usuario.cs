using Hackaton.Core.Entities.Roles.Domain;
using Hackaton.Core.Enumerators;
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

        [BsonElement("senhaHash")]
        public string SenhaHash { get; private set; }

        [BsonElement("role")]
        public ERole Role { get; set; }

        public void CriptografarSenha(string senha)
        {
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool ValidarSenha(string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
        }
    }
}
