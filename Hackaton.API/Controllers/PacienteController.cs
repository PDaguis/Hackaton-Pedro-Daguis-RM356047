using Hackaton.API.DTO.Inputs.Paciente;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteController(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarPacienteInput input)
        {
            try
            {
                var paciente = new Paciente()
                {
                    Cpf = input.Cpf,
                    Nome = input.Nome,
                    Email = input.Email,
                    Genero = input.Genero.HasValue ? input.Genero.Value : EGenero.NaoInformado
                };

                paciente.CriptografarSenha(input.Senha);

                await _pacienteRepository.Cadastrar(paciente);

                return Created();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarPacienteInput input)
        {
            try
            {
                var paciente = await _pacienteRepository.GetById(input.Id);

                if (paciente == null)
                    return NotFound();

                paciente.Cpf = input.Cpf;
                paciente.Nome = input.Nome;
                paciente.Email = input.Email;
                paciente.Genero = input.Genero.HasValue ? input.Genero.Value : EGenero.NaoInformado;

                paciente.CriptografarSenha(input.Senha);

                await _pacienteRepository.Atualizar(paciente);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Deletar(Guid id)
        {
            try
            {
                var paciente = await _pacienteRepository.GetById(id);

                if (paciente == null)
                    return NotFound();

                await _pacienteRepository.Excluir(id);

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("deletar-tudo")]
        public async Task<IActionResult> DeletarTudo()
        {
            try
            {
                await _pacienteRepository.ExcluirTudo();
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var pacientes = await _pacienteRepository.GetAll();

                if (pacientes == null)
                    return NoContent();

                return Ok(pacientes);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var paciente = await _pacienteRepository.GetById(id);

                if (paciente == null)
                    return NotFound();

                return Ok(paciente);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
