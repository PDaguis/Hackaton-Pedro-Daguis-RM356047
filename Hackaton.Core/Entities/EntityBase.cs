using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public abstract class EntityBase
    {
        public EntityBase()
        {
            Id = Guid.NewGuid();
            CadastradoEm = DateTime.Now;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("entityId")]
        public ObjectId EntityId { get; set; }

        [BsonElement("id")]
        public Guid Id { get; private set; }

        [BsonElement("cadastradoEm")]
        public DateTime CadastradoEm { get; private set; }
    }
}
