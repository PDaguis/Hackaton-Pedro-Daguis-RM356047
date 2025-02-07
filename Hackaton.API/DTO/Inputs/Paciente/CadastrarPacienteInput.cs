using Hackaton.Core.Enumerators;
using MongoDB.Bson.Serialization.Attributes;

namespace Hackaton.API.DTO.Inputs.Paciente
{
    public class CadastrarPacienteInput
    {
        public required string Cpf { get; set; }

        public required string Nome { get; set; }

        public required string Email { get; set; }

        public required string Senha { get; set; }
        public EGenero? Genero { get; set; }
    }
}
