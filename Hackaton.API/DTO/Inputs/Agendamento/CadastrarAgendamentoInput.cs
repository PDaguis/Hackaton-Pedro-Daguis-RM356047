using Hackaton.Core.Entities;

namespace Hackaton.API.DTO.Inputs.Agendamento
{
    public class CadastrarAgendamentoInput
    {
        public required Guid MedicoId { get; set; }
        public required DateTime Data { get; set; }
        public List<HorarioAgenda> Horarios { get; set; }
    }
}
