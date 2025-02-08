using FluentValidation;
using Hackaton.API.DTO.Inputs.Usuario;

namespace Hackaton.API.Validators.Usuario
{
    public class CadastrarInputValidator : AbstractValidator<CadastrarInput>
    {
        public CadastrarInputValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .MaximumLength(100).WithMessage("Email deve ter no máximo 100 caracteres")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .Length(6, 20).WithMessage("Senha deve ter entre 6 e 20 caracteres");
        }
    }
}
