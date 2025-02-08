using MongoDB.Bson;
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

        [BsonElement("medicoId")]
        public Guid MedicoId { get; set; }

        [BsonElement("dataHora")]
        public DateTime DataHora { get; set; }

        [BsonElement("disponivel")]
        public bool Disponivel { get; set; } = true;

        public void BloquearHorario()
        {
            Disponivel = false;
        }
        public void LiberarHorario()
        {
            Disponivel = true;
        }
    }
}
