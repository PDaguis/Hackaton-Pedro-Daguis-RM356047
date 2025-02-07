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

        [BsonElement("horarios")]
        public List<HorarioAgenda> Horarios { get; set; }
        public DateTime Data { get; set; }

        public void AddHorario(HorarioAgenda horario)
        {
            if (horario == null)
                return;

            if (Horarios == null)
                Horarios = new List<HorarioAgenda>();

            Horarios.Add(horario);
        }

        public IEnumerable<HorarioAgenda> ListarHorariosDisponiveis()
        {
            return Horarios.Where(x => x.Disponivel == true).AsEnumerable();
        }
    }
}
