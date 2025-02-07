using Hackaton.Core.Enumerators;

namespace Hackaton.API.DTO.Inputs.Medico
{
    public class AtualizarMedicoInput
    {
        public Guid Id { get; set; }
        public string Crm { get; set; }
        public List<EEspecialidade> Especialidades { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }
}
