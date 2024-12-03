
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.PacienteRepository
{
    public interface IPacienteRepository
    {
        // CRUD - CREATE, READ, UPDATE e DELETE
        Task<ServerResponse<UsuarioSaidaDTO>> CreatePaciente(RegisterPacienteDTO model);
        Task<ServerResponse<List<UsuarioSaidaDTO>>> GetPacientes();
        Task<ServerResponse<UsuarioSaidaDTO>> GetPacienteById(int id);
      
        Task<ServerResponse<UsuarioSaidaDTO>> UpdatePaciente(int id, PerfilPacienteDTO model);
        Task<ServerResponse<bool>> DeletePaciente(int id);
      
    }
}
