using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.DoencasRepository
{
    public interface IDoencasRepository
    {
        Task<ServerResponse<List<DoencaSaidaDTO>>> GetDoencas(int historicoId);
        Task<ServerResponse<DoencaSaidaDTO>> AddDoenca(int idPaciente, DoencasDTO model);
        Task<ServerResponse<DoencaSaidaDTO>> UpdateDoenca(int id, DoencasDTO model);
    }
}
