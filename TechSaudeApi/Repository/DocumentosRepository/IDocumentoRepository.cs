
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.DocumentosRepository
{
    public interface IDocumentoRepository
    {
        Task<ServerResponse<DocumentoSaidaDTO>> SaveFileAsync(FileDTO model);
        Task<ServerResponse<DocumentoSaidaDTO>> GetDocumento(int id);
        Task<ServerResponse<List<DocumentoSaidaDTO>>> GetExames(int pacienteId);
        Task<ServerResponse<List<DocumentoSaidaDTO>>> GetReceitas(int pacienteId);
        Task<ServerResponse<List<DocumentoSaidaDTO>>> GetOutros(int pacienteId);


    }
}
