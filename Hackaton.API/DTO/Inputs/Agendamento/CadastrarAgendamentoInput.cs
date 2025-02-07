namespace Hackaton.API.DTO.Inputs.Agendamento
{
    public class CadastrarAgendamentoInput
    {
        public required Guid MedicoId { get; set; }
        public required DateTime DataInicio { get; set; }
        public required DateTime DataLimite { get; set; }
    }
}
