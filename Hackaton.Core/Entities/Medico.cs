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
    public class Medico : Usuario
    {
        [BsonElement("crm")]
        public required string Crm { get; set; }

        [BsonRepresentation(BsonType.String)]
        [BsonElement("especialidade")]
        public required EEspecialidade Especialidade { get; set; }

        [BsonIgnore]
        public virtual List<Agendamento>? Agendas { get; set; }

        public void AddAgenda(Agendamento agenda)
        {
            if (agenda == null)
                return;

            if (Agendas == null)
                Agendas = new List<Agendamento>();

            Agendas.Add(agenda);
        }

        public void RemoveAgenda(Agendamento agenda)
        {
            if (agenda == null)
                return;

            if (Agendas == null)
                return;

            Agendas.Remove(agenda);
        }

        public IEnumerable<Agendamento> ListarHorariosDisponiveis()
        {
            return Agendas;
        }
    }
}
