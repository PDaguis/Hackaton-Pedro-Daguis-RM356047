using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton.Core.Entities
{
    public class HorarioAgenda
    {
        [BsonElement("hora")]
        public DateTime Hora { get; set; }

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
