using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.MedicoRepository;

namespace TechSaude.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicosController(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        // Endpoint para obter todos os médicos
        [HttpGet]
        public async Task<ActionResult<ServerResponse<List<MedicoDTO>>>> GetMedicos()
        {
            var response = await _medicoRepository.GetMedicos();
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        // Endpoint para obter um médico pelo ID
        [HttpGet("{medicoId}")]
        public async Task<ActionResult<ServerResponse<MedicoDTO>>> GetMedicoById([FromRoute] int medicoId)
        {
            var response = await _medicoRepository.GetMedicoById(medicoId);
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Endpoint para registrar um novo médico
        [HttpPost]
        public async Task<ActionResult<ServerResponse<MedicoDTO>>> Register([FromBody] RegisterMedicoDTO model)
        {
            var response = await _medicoRepository.CreateMedico(model);
            if (response.Sucesso)
            {
                return CreatedAtAction(nameof(GetMedicoById), new { medicoId = response.Data!.Id }, response);
            }
            return BadRequest(response);
        }

        // Endpoint para atualizar um médico existente
        [HttpPut("{medicoId}")]
        public async Task<ActionResult<ServerResponse<MedicoDTO>>> UpdateMedico([FromRoute] int medicoId, [FromBody] Medico model)
        {
            var response = await _medicoRepository.UpdateMedico(medicoId, model);
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        // Endpoint para deletar um médico
        [HttpDelete("{medicoId}")]
        public async Task<ActionResult<ServerResponse<bool>>> DeleteMedico([FromRoute] int medicoId)
        {
            var response = await _medicoRepository.DeleteMedico(medicoId);
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("especialidades")]
        public async Task<ActionResult<ServerResponse<List<EspecialidadeDTO>>>> GetEspecialidades()
        {
            var response = await _medicoRepository.GetEspecialidades();
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("localidades")]
        public async Task<ActionResult<ServerResponse<List<LocalidadeDTO>>>> GetLocalidades([FromQuery] string especialidade)
        {
            var response = await _medicoRepository.GetLocalidades(especialidade);
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("filtrar")]

        public async Task<ActionResult<ServerResponse<List<MedicoDTO>>>> FiltrarMedicos([FromQuery] string especialidade, [FromQuery] string localidade)
        {
            var response = await _medicoRepository.FiltrarMedicos(especialidade, localidade);
            if (response.Sucesso)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}