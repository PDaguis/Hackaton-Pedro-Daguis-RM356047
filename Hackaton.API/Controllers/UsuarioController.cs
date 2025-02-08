using Hackaton.API.DTO.Inputs.Usuario;
using Hackaton.API.Security;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System.Numerics;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly TokenProvider _tokenProvider;

        public UsuarioController(IUsuarioRepository usuarioRepository, TokenProvider tokenProvider)
        {
            _usuarioRepository = usuarioRepository;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Login([FromBody] LoginInput input)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorDocumento(input.Documento);

                if (usuario == null)
                    return NotFound();

                if (!usuario.ValidarSenha(input.Senha))
                    return BadRequest("Senha inválida!");

                var token = GenerateToken(usuario);

                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarInput input)
        {
            try
            {
                var todos = await _usuarioRepository.GetAll();
                var usuarioExiste = await _usuarioRepository.ObterPorEmail(input.Email);

                if (usuarioExiste != null)
                    return BadRequest("Usuário já existe");

                Usuario usuario = input.Role switch
                {
                    ERole.Paciente => new Paciente
                    {
                        Nome = input.Nome,
                        Email = input.Email,
                        Role = ERole.Paciente,
                        Cpf = input.Cpf,
                        Genero = input.Genero,
                    },
                    ERole.Medico => new Medico
                    {
                        Nome = input.Nome,
                        Email = input.Email,
                        Role = ERole.Medico,
                        Crm = input.Crm,
                        Especialidade = input.Especialidade.Value
                    },
                    _ => throw new ArgumentException("Regra não especificada")
                };

                usuario.CriptografarSenha(input.Senha);

                await _usuarioRepository.Cadastrar(usuario);

                var token = GenerateToken(usuario);

                return Created(string.Empty, token);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        protected string GenerateToken(Usuario usuario)
        {
            return _tokenProvider.GenerateToken(usuario);
        }
    }
}
