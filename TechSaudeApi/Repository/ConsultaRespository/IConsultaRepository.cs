using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.ConsultaRespository
{
    public interface IConsultaRepository
    {
        Task<ServerResponse<ConsultaSaidaDTO>> GetConsultaById(int id);
        Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultas();
        Task<ServerResponse<Consulta>> AgendarConsulta(int pacienteId, AgendamentoConsultaDTO model);
        Task<ServerResponse<bool>> DeleteConsulta(int id);
        Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasByPaciente(int pacienteId);
        Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasConfirmadasByPaciente(int pacienteId);
        Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasByMedico(int medicoId);
    }
}
