using FluentValidation;
using Hackaton.API.DTO.Inputs.Usuario;

namespace Hackaton.API.Validators.Usuario
{
    public class LoginInputValidator : AbstractValidator<LoginInput>
    {
        public LoginInputValidator()
        {
            RuleFor(x => x.Documento)
                .NotEmpty()
                .WithMessage("Documento é obrigatório");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .Length(6, 20).WithMessage("Senha deve ter entre 6 e 20 caracteres");
        }
    }
}
