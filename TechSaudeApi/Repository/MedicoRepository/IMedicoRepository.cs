
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.MedicoRepository
{
    public interface IMedicoRepository
    {
        // CRUD
        Task<ServerResponse<List<Medico>>> GetMedicos();
        Task<ServerResponse<Medico>> GetMedicoById(int id);
        Task<ServerResponse<Medico>> CreateMedico(RegisterMedicoDTO model);
        Task<ServerResponse<Medico>> UpdateMedico(int id, Medico model);
        Task<ServerResponse<bool>> DeleteMedico(int id); 

        // Funcionalidade Específicas
        Task<ServerResponse<Consulta>> ConfirmarConsulta(int consultaId);

        Task<ServerResponse<List<EspecialidadeDTO>>> GetEspecialidades();
        Task<ServerResponse<List<LocalidadeDTO>>> GetLocalidades(string especialidade);
        Task<ServerResponse<List<MedicoDTO>>> FiltrarMedicos([FromQuery] string especialidade, [FromQuery] string localidade);
    }
}
