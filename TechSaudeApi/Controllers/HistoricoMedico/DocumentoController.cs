using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.DocumentosRepository;

namespace TechSaude.Server.Controllers.HistoricoMedico
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentoController : ControllerBase
    {
        private readonly IDocumentoRepository _fileService;
        private readonly AppDbContext _context;

        public DocumentoController(IDocumentoRepository fileService, AppDbContext context)
        {
            _fileService = fileService;
            _context = context;
        }
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        //[Authorize]
        [HttpPost("upload")]
        public async Task<ServerResponse<DocumentoSaidaDTO>> Upload(FileDTO model)
        {
          return await _fileService.SaveFileAsync(model);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public async Task<ServerResponse<DocumentoSaidaDTO>> GetDocumento(int id)
        {
            return await _fileService.GetDocumento(id);  
        }
        //[Authorize]
        [HttpGet("exames")]
        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetExames(int pacienteId)
        {
            return await _fileService.GetExames(pacienteId);
        }
        //[Authorize]
        [HttpGet("receitas")]
        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetReceitas(int pacienteId)
        {
            return await _fileService.GetReceitas(pacienteId);
        }
        //[Authorize]
        [HttpGet("outros")]
        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetOutrosDoc(int pacienteId)
        {
            return await _fileService.GetOutros(pacienteId);
        }
        //[Authorize]
        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(int id,  [FromServices] IWebHostEnvironment env)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
                return NotFound(new { message = "Documento não encontrado." });

            var uploadsPath = Path.Combine(env.ContentRootPath, "Uploads");
            var filePath = Path.Combine(uploadsPath, documento.Caminho!);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { message = "Arquivo não encontrado no servidor." });

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, documento.TipoArquivo ?? "application/octet-stream", documento.Nome);
        }
    }
}
