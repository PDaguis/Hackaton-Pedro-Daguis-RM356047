using FluentValidation;
using Hackaton.API.DTO.Inputs.Agendamento;
using Hackaton.API.DTO.Inputs.Usuario;
using Hackaton.Core.Entities;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly ILogger<AgendamentoController> _logger;

        private readonly IValidator<CadastrarAgendamentoInput> _validator;

        public AgendamentoController(IAgendamentoRepository agendamentoRepository, ILogger<AgendamentoController> logger, IValidator<CadastrarAgendamentoInput> validator)
        {
            _agendamentoRepository = agendamentoRepository;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Endpoint para cadastrar um agendamento do médico
        /// </summary>
        /// <param name="input">Objeto de input para criar o agendamento</param>
        /// <returns>Retorna OK(200) caso sucesso ou status code de erro</returns>
        [HttpPost("{medicoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Cadastrar([FromBody] IEnumerable<CadastrarAgendamentoInput> inputs, Guid medicoId)
        {
            try
            {
                foreach (var item in inputs)
                {
                    var validationResult = _validator.Validate(item);

                    if (!validationResult.IsValid) { return BadRequest(validationResult.Errors); }

                    var agendamento = new Agendamento()
                    {
                        MedicoId = medicoId,
                        DataHora = item.DataHora
                    };

                    _logger.LogInformation($"Cadastrando agendamento para o médico {medicoId} na data {item.DataHora}");
                    await _agendamentoRepository.Cadastrar(agendamento);
                }
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Atualizar([FromBody] IEnumerable<AtualizarAgendamentoInput> inputs)
        {
            try
            {
                foreach (var input in inputs)
                {
                    _logger.LogInformation($"Obtendo agendamento {input.AgendamentoId}...");
                    var agendamento = await _agendamentoRepository.GetById(input.AgendamentoId);

                    if (agendamento == null)
                    {
                        _logger.LogError($"Agendamento {input.AgendamentoId} não encontrado");
                        return NotFound();
                    }

                    agendamento.DataHora = input.NovaDataHora;

                    _logger.LogInformation($"Atualizando agendamento {input.AgendamentoId} para a nova data {input.NovaDataHora}");
                    await _agendamentoRepository.Atualizar(agendamento);
                }

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                _logger.LogInformation("Obtendo agendamentos...");
                var agendamentos = await _agendamentoRepository.GetAll();

                if (agendamentos == null)
                {
                    _logger.LogInformation("Nenhum agendamento encontrado");
                    return NoContent();
                }

                return Ok(agendamentos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("horarios-por-medico/{medicoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterHorariosDisponiveisPorMedico(Guid medicoId, ObterHorariosDisponiveisMedicoInput input)
        {
            try
            {
                _logger.LogInformation($"Obtendo horários disponíveis para o médico {medicoId} entre {input.DataInicio} e {input.DataFinal}");
                var horarios = await _agendamentoRepository.ObterHorariosDisponiveisPorMedico(medicoId, input.DataInicio, input.DataFinal);

                if (horarios == null)
                {
                    _logger.LogInformation("Nenhum horário disponível encontrado");
                    return NoContent();
                }

                return Ok(horarios);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("excluir-tudo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico")]
        public async Task<IActionResult> ExcluirTudo()
        {
            try
            {
                _logger.LogInformation("Excluindo todos os agendamentos...");
                await _agendamentoRepository.ExcluirTudo();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
