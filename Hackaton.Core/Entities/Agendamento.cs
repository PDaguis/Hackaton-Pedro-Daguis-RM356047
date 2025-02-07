using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public class Agendamento : EntityBase
    {
        // talvez não precise
        public virtual Consulta? Consulta { get; set; }

        [BsonElement("consultaId")]
        public  Guid? ConsultaId { get; set; }

        public virtual Medico Medico { get; set; }

        [BsonElement("medicoId")]
        public required Guid MedicoId { get; set; }

        [BsonElement("dataInicio")]
        public required DateTime DataInicio { get; set; }

        [BsonElement("dataLimite")]
        public required DateTime DataLimite { get; set; }
    }
}
