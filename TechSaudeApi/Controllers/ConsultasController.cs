
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.ConsultaRespository;

namespace TechSaude.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IConsultaRepository _consultaRepository;

        public ConsultasController(AppDbContext context, IConsultaRepository consultaRepository)
        {
            _context = context;
            _consultaRepository = consultaRepository;
        }

        [HttpGet]
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultas()
        {
            return await _consultaRepository.GetConsultas();
        }
        [HttpGet("{consultaId}")]
        public async Task<ServerResponse<ConsultaSaidaDTO>> GetConsultaById(int consultaId)
        {
            return await _consultaRepository.GetConsultaById(consultaId);
        }
        [HttpGet("medico/{medicoId}")]

        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultas([FromRoute] int medicoId)
        {
            return await _consultaRepository.GetConsultasByMedico(medicoId);
        }

        [HttpDelete]

        public async Task<ServerResponse<bool>> DeleteConsulta(int consultaId)
        {
            return await _consultaRepository.DeleteConsulta(consultaId);
        }

        // AUTENTIFICAS

        //[Authorize]
        [HttpPost("agendamento/{pacienteId}")]
        public async Task<ServerResponse<Consulta>> AgendamentoConsulta(int pacienteId, [FromBody] AgendamentoConsultaDTO agendamentoConsulta)
        {
            return await _consultaRepository.AgendarConsulta(pacienteId, agendamentoConsulta);

        }
   
        //[Authorize]
        [HttpGet("paciente/{pacienteId}")]
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> ConsultasPaciente(int pacienteId)
        {
            return await _consultaRepository.GetConsultasByPaciente(pacienteId);

        }
        //[Authorize]
        [HttpGet("confirmadas")]
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> ConsultasConfirmadasPaciente(int pacienteId)
        {
            return await _consultaRepository.GetConsultasConfirmadasByPaciente(pacienteId);
        }
      
    }
}
