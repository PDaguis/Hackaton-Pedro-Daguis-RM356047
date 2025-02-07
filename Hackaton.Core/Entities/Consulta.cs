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
        // talvez não precise
        public virtual Medico Medico { get; set; }
        public required Guid MedicoId { get; set; }

        public virtual Paciente Paciente { get; set; }

        [BsonElement("pacienteId")]
        public required Guid PacienteId { get; set; }

        public virtual Agendamento Agendamento { get; set; }

        [BsonElement("agendamentoId")]
        public required Guid AgendamentoId { get; set; }

        [BsonElement("valor")]
        [BsonRepresentation(MongoDB.Bson.BsonType.Decimal128)]
        public required decimal Valor { get; set; }

        [BsonElement("finalizado")]
        public bool Finalizado { get; set; }

        [BsonElement("aprovado")]
        public bool Aprovado { get; set; }

        [BsonElement("motivoRecusa")]
        public string? MotivoRecusa { get; set; }
    }
}
