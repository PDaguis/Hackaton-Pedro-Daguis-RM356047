namespace Hackaton.API.DTO.Inputs.Usuario
{
    public class LoginInput
    {
        public required string Documento { get; set; }
        public required string Senha { get; set; }
    }
}
