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
        [BsonElement("medicoId")]
        public Guid MedicoId { get; set; }

        [BsonElement("pacienteId")]
        public Guid PacienteId { get; set; }

        [BsonElement("agendaId")]
        public Guid AgendaId { get; set; }

        [BsonElement("finalizado")]
        public bool Finalizado { get; set; } = false;

        [BsonElement("motivoRecusa")]
        public string? MotivoRecusa { get; set; }

        [BsonElement("data")]
        public DateTime Data { get; init; } = DateTime.Now;

        [BsonElement("valor")]
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Valor { get; set; }

        [BsonElement("statusConsulta")]
        public List<StatusConsulta> StatusConsulta { get; set; }

        public void Cancelar(string? motivo)
        {
            if (StatusConsulta.Exists(x => x.Status == EStatusConsulta.Finalizada))
                throw new InvalidOperationException("Não é possível cancelar uma consulta finalizada");

            Finalizado = true;
            MotivoRecusa = motivo;

            AddStatus(EStatusConsulta.Cancelada);
        }

        public void Finalizar()
        {
            Finalizado = true;

            AddStatus(EStatusConsulta.Finalizada);
        }

        public void Aprovar()
        {
            if (!StatusConsulta.Exists(x => x.Status == EStatusConsulta.Solicitada))
                throw new InvalidOperationException("Não é possível aprovar uma consulta que não está solicitada");

            if (StatusConsulta.Exists(x => x.Status == EStatusConsulta.Finalizada) || StatusConsulta.Exists(x => x.Status == EStatusConsulta.Cancelada))
                throw new InvalidOperationException("Não é possível aprovar uma consulta finalizada ou cancelada");

            AddStatus(EStatusConsulta.Aprovada);
        }

        public void VoltarParaSolicitada()
        {
            StatusConsulta.RemoveAll(x => x.Status != EStatusConsulta.Solicitada);
        }

        public void AddStatus(EStatusConsulta? status)
        {
            if (status == null)
                return;

            if (StatusConsulta == null)
                StatusConsulta = new List<StatusConsulta>();

            StatusConsulta.Add(new StatusConsulta()
            {
                Status = status.Value
            });
        }
    }
}
