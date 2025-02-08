using Hackaton.API.DTO.Inputs.Usuario;
using Hackaton.API.Security;
using Hackaton.Core.Entities;
using Hackaton.Core.Entities.Roles.Domain;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;
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
        private readonly IUsuarioRoleRepository _usuarioRoleRepository;
        private readonly TokenProvider _tokenProvider;

        public UsuarioController(IUsuarioRepository usuarioRepository, TokenProvider tokenProvider, IUsuarioRoleRepository usuarioRoleRepository)
        {
            _usuarioRepository = usuarioRepository;
            _tokenProvider = tokenProvider;
            _usuarioRoleRepository = usuarioRoleRepository;
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

                var token = await _tokenProvider.GenerateToken(usuario);

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
                        Role = ERole.Medico ,
                        Crm = input.Crm,
                        Especialidade = input.Especialidade.Value
                    },
                    _ => throw new ArgumentException("Regra não especificada")
                };

                usuario.CriptografarSenha(input.Senha);

                await _usuarioRepository.Cadastrar(usuario);

                var usuarioRole = new UsuarioRole
                {
                    RoleId = usuario.Role == ERole.Paciente ? Role.PacienteId : Role.MedicoId,
                    UsuarioId = usuario.Id,
                    Usuario = usuario
                };

                await _usuarioRoleRepository.Cadastrar(usuarioRole);

                return Created();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
