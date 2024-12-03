using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.VacinaRepository;

namespace TechSaude.Server.Controllers.HistoricoMedico
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacinasController : ControllerBase
    {
        public readonly IVacinaRepository _vacinaRepository;

        public VacinasController(IVacinaRepository vacina)
        {
            _vacinaRepository = vacina;            
        }
        //[Authorize]
        [HttpGet("{idPaciente}")]
        public async Task<ServerResponse<List<VacinaSaidaDTO>>> GetVacinasByPaciente(int idPaciente)
        {
            return await _vacinaRepository.GetVacinas(idPaciente);
        }
        //[Authorize]
        [HttpPost("{idPaciente}")]
        public async Task<ServerResponse<VacinaSaidaDTO>> AddVacinasByPaciente(int idPaciente, [FromBody] VacinaDTO model)
        {
            return await _vacinaRepository.AddVacina(idPaciente, model);
        }
        //[Authorize]
        [HttpPut("{idVacina}")]
        public async Task<ServerResponse<VacinaSaidaDTO>> UpdateVacinaByPaciente(int idVacina, [FromBody] VacinaDTO model)
        {
            return await _vacinaRepository.UpdateVacina(idVacina, model);
        }

    }
}
