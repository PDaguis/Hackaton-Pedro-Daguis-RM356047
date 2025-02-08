﻿using Hackaton.API.DTO.Inputs.Medico;
using Hackaton.Core.Entities;
using Hackaton.Core.Enumerators;
using Hackaton.Core.Interfaces;
using Hackaton.Infra.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hackaton.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicoController(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Cadastrar([FromBody] CadastrarMedicoInput input)
        {
            try
            {
                var medico = new Medico()
                {
                    Crm = input.Crm,
                    Email = input.Email,
                    Nome = input.Nome,
                    Especialidade = input.Especialidade
                };

                medico.CriptografarSenha(input.Senha);

                await _medicoRepository.Cadastrar(medico);

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
        [ProducesResponseType(500)]
        public async Task<IActionResult> Atualizar([FromBody] AtualizarMedicoInput input)
        {
            try
            {
                var medico = await _medicoRepository.GetById(input.Id);

                if (medico == null)
                    return NotFound();

                medico.Crm = input.Crm;
                medico.Email = input.Email;
                medico.Nome = input.Nome;
                medico.Especialidade = input.Especialidade;

                medico.CriptografarSenha(input.Senha);

                await _medicoRepository.Atualizar(medico);

                return Ok();
            }
            catch (Exception e)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var medico = await _medicoRepository.GetById(id);

                if (medico == null)
                    return NotFound();

                await _medicoRepository.Excluir(id);

                return Ok();
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
                await _medicoRepository.ExcluirTudo();
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
                var medicos = await _medicoRepository.GetAll();

                if (medicos == null)
                    return NoContent();

                return Ok(medicos);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            try
            {
                var medico = await _medicoRepository.GetById(id);

                if (medico == null)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("crm/{crm}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorCrm(string crm)
        {
            try
            {
                var medico = await _medicoRepository.ObterPorCrm(crm);

                if (medico == null)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("nome/{nome}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorNome(string nome)
        {
            try
            {
                var medico = await _medicoRepository.ObterPorNome(nome);

                if (medico == null)
                    return NotFound();

                return Ok(medico);
            }
            catch (Exception e)
            {
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
                var especialidades = await _medicoRepository.ListarEspecialidades();

                if (especialidades == null)
                    return NoContent();

                return Ok(especialidades);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("especialidade/{especialidade}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObterPorEspecialidade(EEspecialidade especialidade)
        {
            try
            {
                var medicos = await _medicoRepository.ObterPorEspecialidade(especialidade);

                if (medicos == null)
                    return NoContent();

                return Ok(medicos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
