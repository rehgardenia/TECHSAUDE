
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.PacienteRepository;

namespace TechSaude.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacientesController(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        [HttpGet]
        public async Task<ServerResponse<List<UsuarioSaidaDTO>>> GetPacientes()
        {
            return await _pacienteRepository.GetPacientes();
        }
        [HttpGet]
        [Route("{pacienteId}")]
        public async Task<ServerResponse<UsuarioSaidaDTO>> GetPacienteById(int pacienteId)
        {
            return await _pacienteRepository.GetPacienteById(pacienteId);
        }

        [HttpPost]
        public async Task<ServerResponse<UsuarioSaidaDTO>> AddPaciente([FromBody] RegisterPacienteDTO model)
        {
            return await _pacienteRepository.CreatePaciente(model);
        }

        [HttpPut]
        [Route("{pacienteId}")]
        public async Task<ServerResponse<UsuarioSaidaDTO>> UpdatePaciente(int pacienteId, PerfilPacienteDTO model)
        {
            return await _pacienteRepository.UpdatePaciente(pacienteId, model);
        }
        [HttpDelete]
        [Route("{pacienteId}")]
        public async Task<ServerResponse<bool>> DeleteMedico(int pacienteId)
        {
            return await _pacienteRepository.DeletePaciente(pacienteId);
        }

    }
}