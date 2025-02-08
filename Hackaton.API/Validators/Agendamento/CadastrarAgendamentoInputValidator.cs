using FluentValidation;
using Hackaton.API.DTO.Inputs.Agendamento;

namespace Hackaton.API.Validators.Agendamento
{
    public class CadastrarAgendamentoInputValidator : AbstractValidator<CadastrarAgendamentoInput>
    {
        public CadastrarAgendamentoInputValidator()
        {
            RuleFor(x => x.DataHora)
                .NotEmpty().WithMessage("DataHora é obrigatório")
                .Must(EhDataHoraValida).WithMessage("Não pode ser uma data passada");
        }

        public static bool EhDataHoraValida(DateTime dataHora)
        {
            return DateTime.Now < dataHora;
        }
    }
}
