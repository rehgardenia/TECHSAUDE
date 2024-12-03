using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.AlergiasRepository

{
    public interface IAlergiasRepository
    {
        Task<ServerResponse<List<AlergiaSaidaDTO>>> GetAlergias(int historicoId);
        Task<ServerResponse<AlergiaSaidaDTO>> AddAlergia(int id, AlergiasDTO model);
        Task<ServerResponse<AlergiaSaidaDTO>> UpdateAlergia(int id, AlergiasDTO model);
    }
}
