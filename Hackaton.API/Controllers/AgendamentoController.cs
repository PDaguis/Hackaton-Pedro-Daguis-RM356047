using Hackaton.API.DTO.Inputs.Agendamento;
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
    [Authorize]
    public class AgendamentoController : ControllerBase
    {
        private readonly IAgendamentoRepository _agendamentoRepository;

        public AgendamentoController(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
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
                    var agendamento = new Agendamento()
                    {
                        MedicoId = medicoId,
                        DataHora = item.DataHora
                    };

                    await _agendamentoRepository.Cadastrar(agendamento);
                }

                return Created();
            }
            catch (Exception e)
            {
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
                    var agendamento = await _agendamentoRepository.GetById(input.AgendamentoId);

                    if (agendamento == null)
                        return NotFound();

                    agendamento.DataHora = input.NovaDataHora;

                    await _agendamentoRepository.Atualizar(agendamento);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var agendamentos = await _agendamentoRepository.GetAll();

                if (agendamentos == null)
                    return NoContent();

                return Ok(agendamentos);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("horarios-por-medico/{medicoId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterHorariosDisponiveisPorMedico(Guid medicoId, ObterHorariosDisponiveisMedicoInput input)
        {
            try
            {
                var horarios = await _agendamentoRepository.ObterHorariosDisponiveisPorMedico(medicoId, input.DataInicio, input.DataFinal);

                if(horarios == null)
                    return NoContent();

                return Ok(horarios);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("excluir-tudo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> ExcluirTudo()
        {
            try
            {
                await _agendamentoRepository.ExcluirTudo();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
