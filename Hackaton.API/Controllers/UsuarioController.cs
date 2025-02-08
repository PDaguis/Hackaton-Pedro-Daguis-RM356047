using FluentValidation;
using Hackaton.API.DTO.Inputs.Usuario;
using Hackaton.API.Security;
using Hackaton.API.Validators.Usuario;
using Hackaton.Core.Entities;
using Hackaton.Core.Entities.Roles.Domain;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using System.Linq;
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
        private readonly ILogger<MedicoController> _logger;
        private readonly IRoleRepository _roleRepository;

        private readonly IValidator<CadastrarInput> _validatorRegister;
        private readonly IValidator<LoginInput> _validatorLogin;

        public UsuarioController(IUsuarioRepository usuarioRepository, TokenProvider tokenProvider, IUsuarioRoleRepository usuarioRoleRepository, ILogger<MedicoController> logger, IRoleRepository roleRepository, IValidator<CadastrarInput> validatorRegister, IValidator<LoginInput> validatorLogin)
        {
            _usuarioRepository = usuarioRepository;
            _tokenProvider = tokenProvider;
            _usuarioRoleRepository = usuarioRoleRepository;
            _logger = logger;
            _roleRepository = roleRepository;
            _validatorRegister = validatorRegister;
            _validatorLogin = validatorLogin;
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
                var validationResult = _validatorLogin.Validate(input);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToDictionary());

                _logger.LogInformation($"Obtendo usuário por documento {input.Documento}...");
                var usuario = await _usuarioRepository.ObterPorDocumento(input.Documento);

                if (usuario == null)
                {
                    _logger.LogError("Usuário não encontrado");
                    return NotFound("Usuário não encontrado");
                }

                _logger.LogInformation("Validando senha...");
                if (!usuario.ValidarSenha(input.Senha))
                    return BadRequest("Senha inválida!");

                _logger.LogInformation("Gerando token...");
                var token = await _tokenProvider.GenerateToken(usuario);

                _logger.LogInformation("Login e Token gerado com sucesso!");
                return Ok(token);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
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
                var validationResult = _validatorRegister.Validate(input);

                if (!validationResult.IsValid)
                    return BadRequest(validationResult.ToDictionary());

                _logger.LogInformation($"Verificando se usuário {input.Email} já existe...");
                var usuarioExiste = await _usuarioRepository.ObterPorEmail(input.Email);

                if (usuarioExiste != null)
                {
                    _logger.LogError("Usuário já existe");
                    return BadRequest("Usuário já existe");
                }

                _logger.LogInformation($"Montando objeto usuário...");
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
                    ERole.Administrador => new Administrador
                    {
                        Nome = input.Nome,
                        Email = input.Email,
                        Role = ERole.Administrador
                    },
                    _ => throw new ArgumentException("Regra não especificada")
                };
                _logger.LogInformation($"Usuário {usuario.Role.GetDisplayName()}...");

                _logger.LogInformation($"Criptografando senha...");
                usuario.CriptografarSenha(input.Senha);

                _logger.LogInformation($"Cadastrando usuário...");
                await _usuarioRepository.Cadastrar(usuario);

                _logger.LogInformation($"Montando objeto role do usuário...");
                var usuarioRole = new UsuarioRole
                {
                    RoleId = (int)usuario.Role,
                    UsuarioId = usuario.Id,
                    Usuario = usuario
                };

                _logger.LogInformation($"Cadastrando role do usuário...");
                await _usuarioRoleRepository.Cadastrar(usuarioRole);

                return Created();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost("roles")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> CadastrarRoles()
        {
            try
            {
                _logger.LogInformation("Verificando se roles existem...");
                if (await _roleRepository.Exists())
                {
                    _logger.LogInformation("Roles existem!");
                    return Ok("Roles existem!");
                }

                var pacienteRole = new Role { RoleId = 1, Name = ERole.Paciente };
                var medicoRole = new Role { RoleId = 2, Name = ERole.Medico };
                var adminRole = new Role { RoleId = 3, Name = ERole.Administrador };

                _logger.LogInformation("Cadastrando roles...");
                await _roleRepository.Cadastrar(pacienteRole);
                await _roleRepository.Cadastrar(medicoRole);
                await _roleRepository.Cadastrar(adminRole);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("roles")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterRoles()
        {
            try
            {
                _logger.LogInformation("Obtendo roles...");
                var roles = await _roleRepository.GetAll();

                if (roles == null)
                {
                    _logger.LogInformation("Roles não encontradas");
                    return NoContent();
                }

                return Ok(roles);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
