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
    public class Consulta : EntityBase
    {
        public virtual Medico Medico { get; set; }
        public required Guid MedicoId { get; set; }

        public virtual Paciente Paciente { get; set; }

        [BsonElement("pacienteId")]
        public required Guid PacienteId { get; set; }

        [BsonElement("valor")]
        [BsonRepresentation(BsonType.Decimal128)]
        public required decimal Valor { get; set; }

        [BsonElement("finalizado")]
        public bool Finalizado { get; set; }

        [BsonElement("motivoRecusa")]
        public string? MotivoRecusa { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("statusConsulta")]
        public EStatusConsulta StatusConsulta { get; set; }
    }
}
