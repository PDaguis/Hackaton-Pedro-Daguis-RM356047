using Hackaton.API.DTO.Inputs.Medico;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<MedicoController> _logger;

        public MedicoController(IMedicoRepository medicoRepository, IUsuarioRepository usuarioRepository, ILogger<MedicoController> logger)
        {
            _medicoRepository = medicoRepository;
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico")]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarMedicoInput input)
        {
            try
            {
                _logger.LogInformation($"Obtendo médico {input.Id}...");
                var medico = await _usuarioRepository.ObterMedicoPorId(input.Id);

                if (medico == null)
                {
                    _logger.LogError($"Médico não encontrado");
                    return NotFound();
                }

                medico.Crm = input.Crm;
                medico.Email = input.Email;
                medico.Nome = input.Nome;
                medico.Especialidade = input.Especialidade;

                _logger.LogInformation($"Criptografando senha...");
                medico.CriptografarSenha(input.Senha);

                _logger.LogInformation($"Atualizando médico {input.Id}...");
                await _usuarioRepository.Atualizar(medico);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                _logger.LogInformation($"Obtendo médico {id}...");
                var medico = await _usuarioRepository.GetById(id);

                if (medico == null)
                {
                    _logger.LogError($"Médico não encontrado");
                    return NotFound();
                }

                _logger.LogInformation($"Excluindo médico {id}...");
                await _usuarioRepository.Excluir(id);

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
                _logger.LogInformation("Excluindo todos os médicos...");
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
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                _logger.LogInformation("Listando médicos...");
                var medicos = await _usuarioRepository.Listar(ERole.Medico);

                if (medicos == null)
                {
                    _logger.LogWarning("Nenhum médico encontrado");
                    return NoContent();
                }

                return Ok(medicos);
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
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                _logger.LogInformation($"Obtendo médico {id}...");
                var medico = await _usuarioRepository.GetById(id);

                if (medico == null)
                {
                    _logger.LogInformation($"Médico não encontrado");
                    return NotFound();
                }

                return Ok(medico);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterPorNome(string nome)
        {
            try
            {
                _logger.LogInformation($"Obtendo médico {nome}...");
                var medico = await _usuarioRepository.ObterPorNome(nome);

                if (medico == null)
                {
                    _logger.LogInformation($"Médico não encontrado");
                    return NotFound();
                }

                return Ok(medico);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("crm/{crm}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterPorCrm(string crm)
        {
            try
            {
                _logger.LogInformation($"Obtendo médico pelo CRM {crm}...");
                var medico = await _usuarioRepository.ObterPorCrm(crm);

                if (medico == null)
                {
                    _logger.LogInformation($"Médico não encontrado");
                    return NotFound();
                }

                return Ok(medico);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("especialidades")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterEspecialidades()
        {
            try
            {
                _logger.LogInformation("Listando especialidades...");
                var especialidades = await _medicoRepository.ListarEspecialidades();

                if (especialidades == null)
                {
                    _logger.LogWarning("Nenhuma especialidade encontrada");
                    return NoContent();
                }

                return Ok(especialidades);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("especialidade/{especialidade}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        [Authorize(Roles = "Administrador, Medico, Paciente")]
        public async Task<IActionResult> ObterPorEspecialidade(EEspecialidade especialidade)
        {
            try
            {
                _logger.LogInformation($"Obtendo médicos da especialidade {especialidade}...");
                var medicos = await _usuarioRepository.ObterPorEspecialidade(especialidade);

                if (medicos == null)
                {
                    _logger.LogWarning("Nenhum médico encontrado");
                    return NoContent();
                }

                return Ok(medicos);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
