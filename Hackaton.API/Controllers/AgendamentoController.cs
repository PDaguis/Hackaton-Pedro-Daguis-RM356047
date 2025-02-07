using Hackaton.API.DTO.Inputs.Agendamento;
using Hackaton.Core.Entities;
using Hackaton.Core.Interfaces;
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

        public AgendamentoController(IAgendamentoRepository agendamentoRepository)
        {
            _agendamentoRepository = agendamentoRepository;
        }

        /// <summary>
        /// Endpoint para cadastrar um agendamento do médico
        /// </summary>
        /// <param name="input">Objeto de input para criar o agendamento</param>
        /// <returns>Retorna OK(200) caso sucesso ou status code de erro</returns>
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarAgendamentoInput input)
        {
            try
            {
                var agendamento = new Agendamento()
                {
                    DataInicio = input.DataInicio,
                    DataLimite = input.DataLimite,
                    MedicoId = input.MedicoId
                };

                await _agendamentoRepository.Cadastrar(agendamento);

                return Created();
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
    }
}
