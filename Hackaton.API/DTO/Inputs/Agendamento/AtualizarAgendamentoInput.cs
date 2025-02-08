using Hackaton.Core.Entities;

namespace Hackaton.API.DTO.Inputs.Agendamento
{
    public class AtualizarAgendamentoInput
    {
        public required Guid AgendamentoId { get; set; }
        public required DateTime NovaDataHora { get; set; }
    }
}
