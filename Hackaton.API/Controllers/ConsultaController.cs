using Hackaton.API.DTO.Inputs.Consulta;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultaController : ControllerBase
    {
        private readonly IConsultaRepository _consultaRepository;
        private readonly IAgendamentoRepository _agendamentoRepository;

        public ConsultaController(IConsultaRepository consultaRepository, IAgendamentoRepository agendamentoRepository)
        {
            _consultaRepository = consultaRepository;
            _agendamentoRepository = agendamentoRepository;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Agendar([FromBody] CadastrarConsultaInput input)
        {
            try
            {
                var agendamento = await _agendamentoRepository.ObterDisponivel(input.AgendaId);

                if (agendamento == null)
                    return BadRequest("Agenda indisponível");

                var consulta = new Consulta()
                {
                    MedicoId = input.MedicoId,
                    PacienteId = input.PacienteId,
                    AgendaId = input.AgendaId,
                    Valor = input.Valor,
                    Data = agendamento.DataHora
                };

                consulta.AddStatus(EStatusConsulta.Solicitada);

                await _consultaRepository.Cadastrar(consulta);
                return Created();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("aprovar/{consultaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Aprovar(Guid consultaId)
        {
            var consulta = await _consultaRepository.GetById(consultaId);

            if (consulta == null)
                return NotFound();

            var agendamento = await _agendamentoRepository.GetById(consulta.AgendaId);

            consulta.Aprovar();

            try
            {
                await Atualizar(consulta);

                agendamento.BloquearHorario();

                await _agendamentoRepository.Atualizar(agendamento);

                return Ok();
            }
            catch (Exception e)
            {
                agendamento.LiberarHorario();
                consulta.VoltarParaSolicitada();

                await _agendamentoRepository.Atualizar(agendamento);
                await Atualizar(consulta);

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("finalizar/{consultaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Finalizar(Guid consultaId)
        {
            try
            {
                var consulta = await _consultaRepository.GetById(consultaId);

                if (consulta == null)
                    return NotFound();

                consulta.Finalizar();

                await Atualizar(consulta);

                return Ok();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("cancelar/{consultaId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Cancelar([FromBody] CancelarConsultaInput input, Guid consultaId)
        {
            try
            {
                var consulta = await _consultaRepository.GetById(consultaId);

                if (consulta == null)
                    return NotFound();

                consulta.Cancelar(input.Motivo);

                await Atualizar(consulta);

                var agendamento = await _agendamentoRepository.GetById(consulta.AgendaId);

                agendamento.LiberarHorario();

                await _agendamentoRepository.Atualizar(agendamento);

                return Ok(consulta);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpGet("status/{statusConsulta}/{medicoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ListarPorStatusMedico(EStatusConsulta statusConsulta, Guid medicoId)
        {
            try
            {
                var consultas = await _consultaRepository.GetAllByStatusMedico(statusConsulta, medicoId);

                if (consultas == null)
                    return NoContent();

                return Ok(consultas);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("excluir-tudo")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ExcluirTudo()
        {
            try
            {
                await _consultaRepository.ExcluirTudo();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task Atualizar(Consulta consulta)
        {
            await _consultaRepository.Atualizar(consulta);
        }
    }
}
