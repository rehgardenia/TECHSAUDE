using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.VacinaRepository
{
    public interface IVacinaRepository
    {
        Task<ServerResponse<List<VacinaSaidaDTO>>> GetVacinas(int historicoId);
        Task<ServerResponse<VacinaSaidaDTO>> AddVacina(int idPaciente, VacinaDTO model);
        Task<ServerResponse<VacinaSaidaDTO>> UpdateVacina(int id, VacinaDTO model);
    }
}
