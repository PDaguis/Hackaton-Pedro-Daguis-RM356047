using Hackaton.API.DTO.Inputs.Paciente;
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
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<MedicoController> _logger;

        public PacienteController(IPacienteRepository pacienteRepository, IUsuarioRepository usuarioRepository, ILogger<MedicoController> logger)
        {
            _pacienteRepository = pacienteRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Paciente")]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarPacienteInput input)
        {
            try
            {
                _logger.LogInformation($"Obtendo paciente {input.Id}...");
                var paciente = await _usuarioRepository.ObterPacientePorId(input.Id);

                if (paciente == null)
                {
                    _logger.LogError($"Paciente não encontrado");
                    return NotFound();
                }

                paciente.Cpf = input.Cpf;
                paciente.Nome = input.Nome;
                paciente.Email = input.Email;
                paciente.Genero = input.Genero.HasValue ? input.Genero.Value : EGenero.NaoInformado;

                _logger.LogInformation($"Criptografando senha...");
                paciente.CriptografarSenha(input.Senha);

                _logger.LogInformation($"Atualizando paciente {input.Id}...");
                await _usuarioRepository.Atualizar(paciente);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Paciente")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                _logger.LogInformation($"Obtendo paciente {id}...");
                var paciente = await _usuarioRepository.ObterPacientePorId(id);

                if (paciente == null)
                {
                    _logger.LogError($"Paciente não encontrado");
                    return NotFound();
                }

                _logger.LogInformation($"Excluindo paciente {id}...");
                await _pacienteRepository.Excluir(id);

                return Ok();
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
                _logger.LogInformation($"Excluindo todos os pacientes...");
                await _usuarioRepository.ExcluirTudo();

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
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                _logger.LogInformation("Listando pacientes...");
                var pacientes = await _usuarioRepository.Listar(ERole.Paciente);

                if (pacientes == null)
                {
                    _logger.LogError("Nenhum paciente encontrado");
                    return NoContent();
                }

                return Ok(pacientes);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                _logger.LogInformation($"Obtendo paciente {id}...");
                var paciente = await _usuarioRepository.ObterPacientePorId(id);

                if (paciente == null)
                {
                    _logger.LogError($"Paciente não encontrado");
                    return NotFound();
                }

                return Ok(paciente);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
