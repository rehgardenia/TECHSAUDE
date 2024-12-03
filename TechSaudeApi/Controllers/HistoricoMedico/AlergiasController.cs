using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AlergiasRepository;
using TechSaude.Server.Repository.VacinaRepository;

namespace TechSaude.Server.Controllers.HistoricoMedico
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlergiasController : ControllerBase
    {
        public readonly IAlergiasRepository _alergiaRepository;

        public AlergiasController(IAlergiasRepository alergia)
        {
            _alergiaRepository = alergia;            
        }
        //[Authorize]
        [HttpGet("{idPaciente}")]
        public async Task<ServerResponse<List<AlergiaSaidaDTO>>> GetAlergiasByPaciente(int idPaciente)
        {
            return await _alergiaRepository.GetAlergias(idPaciente);
        }
        //[Authorize]
        [HttpPost("{idPaciente}")]
        public async Task<ServerResponse<AlergiaSaidaDTO>> AddAlergiaByPaciente(int idPaciente, [FromBody] AlergiasDTO model)
        {
            return await _alergiaRepository.AddAlergia(idPaciente, model);
        }
        //[Authorize]
        [HttpPut("{idAlergia}")]
        public async Task<ServerResponse<AlergiaSaidaDTO>> UpdateAlergiaByPaciente(int idAlergia, [FromBody] AlergiasDTO model)
        {
            return await _alergiaRepository.UpdateAlergia(idAlergia, model);
        }

    }
}
