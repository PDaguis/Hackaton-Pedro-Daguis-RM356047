using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;

namespace Hackaton.API.DTO.Inputs.Medico
{
    public class CadastrarMedicoInput
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public required string Crm { get; set; }
        public required EEspecialidade Especialidade { get; set; }
    }
}
