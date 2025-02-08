using Hackaton.API.DTO.Inputs.Consulta;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        private readonly ILogger<ConsultaController> _logger;

        public ConsultaController(IConsultaRepository consultaRepository, IAgendamentoRepository agendamentoRepository, ILogger<ConsultaController> logger)
        {
            _consultaRepository = consultaRepository;
            _agendamentoRepository = agendamentoRepository;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Paciente")]
        public async Task<IActionResult> Agendar([FromBody] CadastrarConsultaInput input)
        {
            try
            {
                _logger.LogInformation($"Verificando se existe o agendamento disponível para a agenda {input.AgendaId}...");
                var agendamento = await _agendamentoRepository.ObterDisponivel(input.AgendaId);

                if (agendamento == null)
                {
                    _logger.LogInformation("Agenda indisponível");
                    return BadRequest("Agenda indisponível");
                }

                var consulta = new Consulta()
                {
                    MedicoId = input.MedicoId,
                    PacienteId = input.PacienteId,
                    AgendaId = input.AgendaId,
                    Valor = input.Valor,
                    Data = agendamento.DataHora
                };

                consulta.AddStatus(EStatusConsulta.Solicitada);

                _logger.LogInformation($"Solicitando consulta do paciente {input.PacienteId} para o médico {input.MedicoId}");
                await _consultaRepository.Cadastrar(consulta);
                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPatch("aprovar/{consultaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Aprovar(Guid consultaId)
        {
            _logger.LogInformation($"Obtendo consulta {consultaId}...");
            var consulta = await _consultaRepository.GetById(consultaId);

            if (consulta == null)
            {
                _logger.LogInformation("Consulta não encontrada");
                return NotFound();
            }

            _logger.LogInformation($"Obtendo agendamento {consulta.AgendaId}...");
            var agendamento = await _agendamentoRepository.GetById(consulta.AgendaId);

            if(agendamento == null)
            {
                _logger.LogInformation("Agendamento não encontrado");
                return NotFound();
            }

            _logger.LogError($"Aprovando consulta {consultaId}...");
            consulta.Aprovar();

            try
            {
                _logger.LogError($"Atualizando consulta {consultaId}...");
                await Atualizar(consulta);

                _logger.LogError($"Bloqueando horário do agendamento {consulta.AgendaId}...");
                agendamento.BloquearHorario();

                _logger.LogError($"Atualizando agendamento {consulta.AgendaId}...");
                await _agendamentoRepository.Atualizar(agendamento);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
        [Authorize(Roles = "Medico")]
        public async Task<IActionResult> Finalizar(Guid consultaId)
        {
            try
            {
                _logger.LogInformation($"Obtendo consulta {consultaId}...");
                var consulta = await _consultaRepository.GetById(consultaId);

                if (consulta == null)
                {
                    _logger.LogInformation("Consulta não encontrada");
                    return NotFound();
                }

                _logger.LogInformation($"Finalizando consulta {consultaId}...");
                consulta.Finalizar();

                _logger.LogInformation($"Atualizando consulta {consultaId}...");
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
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> Cancelar([FromBody] CancelarConsultaInput input, Guid consultaId)
        {
            try
            {
                _logger.LogInformation($"Obtendo consulta {consultaId}...");
                var consulta = await _consultaRepository.GetById(consultaId);

                if (consulta == null)
                {
                    _logger.LogInformation("Consulta não encontrada");
                    return NotFound();
                }

                _logger.LogInformation($"Cancelando consulta {consultaId}...");
                consulta.Cancelar(input.Motivo);

                _logger.LogInformation($"Atualizando consulta {consultaId}...");
                await Atualizar(consulta);

                _logger.LogInformation($"Obtendo agendamento {consulta.AgendaId}...");
                var agendamento = await _agendamentoRepository.GetById(consulta.AgendaId);

                _logger.LogInformation($"Liberando horário do agendamento {consulta.AgendaId}...");
                agendamento.LiberarHorario();

                _logger.LogInformation($"Atualizando agendamento {consulta.AgendaId}...");
                await _agendamentoRepository.Atualizar(agendamento);

                return Ok(consulta);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("status/{statusConsulta}/{medicoId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico")]
        public async Task<IActionResult> ListarPorStatusMedico(EStatusConsulta statusConsulta, Guid medicoId)
        {
            try
            {
                _logger.LogInformation($"Obtendo consultas com status {statusConsulta} do médico {medicoId}...");
                var consultas = await _consultaRepository.GetAllByStatusMedico(statusConsulta, medicoId);

                if (consultas == null)
                {
                    _logger.LogInformation("Nenhuma consulta encontrada");
                    return NoContent();
                }

                return Ok(consultas);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("paciente/{pacienteId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Medico")]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ListarPorPaciente(Guid pacienteId)
        {
            try
            {
                _logger.LogInformation($"Obtendo consultas do paciente {pacienteId}...");
                var consultas = await _consultaRepository.GetAllByPaciente(pacienteId);

                if (consultas == null)
                {
                    _logger.LogInformation("Nenhuma consulta encontrada");
                    return NoContent();
                }

                return Ok(consultas);
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
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ExcluirTudo()
        {
            try
            {
                _logger.LogInformation("Excluindo todas as consultas...");
                await _consultaRepository.ExcluirTudo();

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        public async Task Atualizar(Consulta consulta)
        {
            await _consultaRepository.Atualizar(consulta);
        }
    }
}
