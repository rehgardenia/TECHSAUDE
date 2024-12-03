using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.DoencasRepository;
using TechSaude.Server.Repository.VacinaRepository;

namespace TechSaude.Server.Controllers.HistoricoMedico
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoencasController : ControllerBase
    {
        public readonly IDoencasRepository _doencaRepository;

        public DoencasController(IDoencasRepository doenca)
        {
            _doencaRepository = doenca;            
        }
        //[Authorize]
        [HttpGet("{idPaciente}")]
        public async Task<ServerResponse<List<DoencaSaidaDTO>>> GetVacinasByPaciente(int idPaciente)
        {
            return await _doencaRepository.GetDoencas(idPaciente);
        }
        //[Authorize]
        [HttpPost("{idPaciente}")]
        public async Task<ServerResponse<DoencaSaidaDTO>> AddVacinasByPaciente(int idPaciente, [FromBody] DoencasDTO model)
        {
            return await _doencaRepository.AddDoenca(idPaciente, model);
        }
        //[Authorize]
        [HttpPut]
        public async Task<ServerResponse<DoencaSaidaDTO>> UpdateVacinaByPaciente(int idDoenca, [FromBody] DoencasDTO model)
        {
            return await _doencaRepository.UpdateDoenca(idDoenca, model);
        }

    }
}
