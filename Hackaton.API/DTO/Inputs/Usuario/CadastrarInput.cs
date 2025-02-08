using Hackaton.Core.Enumerators;

namespace Hackaton.API.DTO.Inputs.Usuario
{
    public class CadastrarInput
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
        public string? Cpf { get; set; }
        public EGenero? Genero { get; set; }
        public string? Crm { get; set; }
        public EEspecialidade? Especialidade { get; set; }
        public ERole Role { get; set; }
    }
}
