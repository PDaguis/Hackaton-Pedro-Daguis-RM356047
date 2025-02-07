using Hackaton.Core.Enumerators;

namespace Hackaton.API.DTO.Inputs.Paciente
{
    public class AtualizarPacienteInput
    {
        public required Guid Id { get; set; }
        public required string Cpf { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }

        public required string Senha { get; set; }
        public EGenero? Genero { get; set; }
    }
}
