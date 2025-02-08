namespace Hackaton.API.DTO.Inputs.Consulta
{
    public class CadastrarConsultaInput
    {
        public Guid MedicoId { get; set; }
        public Guid PacienteId { get; set; }
        public Guid AgendaId { get; set; }
        public decimal Valor { get; set; }
    }
}
