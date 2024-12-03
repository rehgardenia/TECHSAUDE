using Microsoft.EntityFrameworkCore;
using Mysqlx.Expr;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using static Azure.Core.HttpHeader;

namespace TechSaude.Server.Repository.DocumentosRepository
{
    public class DocumentoRepository : IDocumentoRepository
    {

        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        private readonly ILogger<DocumentoRepository> _logger;

        public DocumentoRepository(IWebHostEnvironment environment, AppDbContext context, ILogger<DocumentoRepository> logger)
        {
            _environment = environment;
            _context = context;
            _logger = logger;
        }

        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        public async Task<ServerResponse<DocumentoSaidaDTO>> SaveFileAsync(FileDTO model)
        {
            try
            {
                // Aguardar o resultado de FirstOrDefaultAsync
                var historico = await _context.HistoricosMedicos.FirstOrDefaultAsync(h => h.PacienteId == model.pacienteId);

                if (historico == null)
                {
                    return CreateResponse<DocumentoSaidaDTO>(false, $"Histórico do Paciente não encontrado", null);
                }

                if (model.file == null || model.file.Length == 0)
                    return CreateResponse<DocumentoSaidaDTO>(false, "Arquivo inválido.", null);

                // Definir o caminho para salvar os arquivos
                var uploadsPath = Path.Combine(_environment.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadsPath))
                    Directory.CreateDirectory(uploadsPath);

                // Gerar um nome único para o arquivo
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.file.FileName);
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // Salvar o arquivo no sistema de arquivos
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.file.CopyToAsync(stream);
                }

                if (!DateTime.TryParse(model.dataAssociada, out DateTime data))
                    return CreateResponse<DocumentoSaidaDTO>(false, "Data de nascimento inválida.", null);
                // Criar a entidade Documento
                var documento = new Documento
                {
                    Nome = model.file.FileName,
                    Categoria = model.categoria,
                    Descricao = model.descricao ?? "Sem descrição",  // Inclua a descrição ou um valor padrão
                    DataAssociada = data,
                    DataUpload = DateTime.UtcNow,
                    Caminho = uniqueFileName, // Armazenar apenas o nome único
                    TamanhoArquivo = model.file.Length,
                    TipoArquivo = model.file.ContentType,
                    HistoricoId = historico.Id // Usar o Id do histórico corretamente
                };

                // Adicionar ao contexto e salvar no banco de dados
                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();

                var documentoSaida = new DocumentoSaidaDTO
                {
                    historicoId = documento.HistoricoId,
                    documentoID = documento.DocumentoId,
                    nome = documento.Nome,
                    categoria = documento.Categoria,
                    descricao = documento.Descricao!,
                    dataAssociada = documento.DataAssociada,
                    dataUpload = documento.DataUpload,
                    caminho = documento.Caminho,
                    tamanhoArquivo = documento.TamanhoArquivo,
                    tipoArquivo = documento.TipoArquivo

                };
                return CreateResponse<DocumentoSaidaDTO>(true, "Arquivo enviado com sucesso.", documentoSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar documento.");
                return CreateResponse<DocumentoSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }


        public async Task<ServerResponse<DocumentoSaidaDTO>> GetDocumento(int id)
        {
            try
            {
                var documento =  await _context.Documentos.FirstOrDefaultAsync(d => d.DocumentoId == id);
                if (documento == null)
                {
                    return CreateResponse<DocumentoSaidaDTO>(false, "Documento não encontrado", null);
                }
                var documentoSaida = new DocumentoSaidaDTO
                {
                    historicoId = documento.HistoricoId,
                    documentoID = documento.DocumentoId,
                    nome = documento.Nome!,
                    categoria = documento.Categoria,
                    descricao = documento.Descricao!,
                    dataAssociada = documento.DataAssociada,
                    dataUpload = documento.DataUpload,
                    caminho = documento.Caminho!,
                    tamanhoArquivo = documento.TamanhoArquivo,
                    tipoArquivo = documento.TipoArquivo!

                };
                return CreateResponse<DocumentoSaidaDTO>(true, "Documento encontrado", documentoSaida);
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Erro ao pegar o  documento.");
                return CreateResponse<DocumentoSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);

            }
            
        }

        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetExames(int pacienteId)
        {
            try
            {
                // Buscar os documentos do paciente que pertencem à categoria 'Exames'
                var exames = await _context.Documentos
                                           .Where(d => d.Historico!.PacienteId == pacienteId && d.Categoria == TipoDocumento.Exames)
                                           .ToListAsync(); // Executa a consulta e obtém a lista

                if (exames == null || !exames.Any()) // Verifica se a lista está vazia
                {
                    return CreateResponse<List<DocumentoSaidaDTO>>(false, "Exames não encontrados.", null);
                }

                // Mapeia os documentos para a lista de DTOs
                var examesSaida = exames.Select(documento => new DocumentoSaidaDTO
                {
                    historicoId = documento.HistoricoId, // Supondo que você tem esta propriedade
                    documentoID = documento.DocumentoId,
                    nome = documento.Nome!,
                    categoria = documento.Categoria,
                    descricao = documento.Descricao!,
                    dataAssociada = documento.DataAssociada,
                    dataUpload = documento.DataUpload,
                    caminho = documento.Caminho!,
                    tamanhoArquivo = documento.TamanhoArquivo,
                    tipoArquivo = documento.TipoArquivo!
                }).ToList();

                return CreateResponse<List<DocumentoSaidaDTO>>(true, "Exames encontrados.", examesSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar exames.");
                return CreateResponse<List<DocumentoSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }



        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetReceitas(int pacienteId)
        {
            try
            {
                // Buscar os documentos do paciente que pertencem à categoria 'Exames'
                var receitas = await _context.Documentos
                                           .Where(d => d.Historico!.PacienteId == pacienteId && d.Categoria == TipoDocumento.Receitas)
                                           .ToListAsync();

                if (receitas == null || receitas.Count == 0)
                {
                    return CreateResponse<List<DocumentoSaidaDTO>>(false, "Receitas não encontrados.", null);
                }
                var receitasSaida = receitas.Select(documento => new DocumentoSaidaDTO
                {
                    historicoId = documento.HistoricoId, // Supondo que você tem esta propriedade
                    documentoID = documento.DocumentoId,
                    nome = documento.Nome!,
                    categoria = documento.Categoria,
                    descricao = documento.Descricao!,
                    dataAssociada = documento.DataAssociada,
                    dataUpload = documento.DataUpload,
                    caminho = documento.Caminho!,
                    tamanhoArquivo = documento.TamanhoArquivo,
                    tipoArquivo = documento.TipoArquivo!
                }).ToList();

                return CreateResponse<List<DocumentoSaidaDTO>>(true, "Receitas encontrados.", receitasSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar receitas.");
                return CreateResponse<List<DocumentoSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<List<DocumentoSaidaDTO>>> GetOutros(int pacienteId)
        {
            try
            {
                // Buscar os documentos do paciente que pertencem à categoria 'Exames'
                var outros = await _context.Documentos
                                           .Where(d => d.Historico!.PacienteId == pacienteId && d.Categoria == TipoDocumento.Outros)
                                           .ToListAsync();

                if (outros == null || outros.Count == 0)
                {
                    return CreateResponse<List<DocumentoSaidaDTO>>(false, "Documentos não encontrados.", null);
                }
               
                var dcoumentosSaida = outros.Select(documento => new DocumentoSaidaDTO
                {
                    historicoId = documento.HistoricoId, // Supondo que você tem esta propriedade
                    documentoID = documento.DocumentoId,
                    nome = documento.Nome!,
                    categoria = documento.Categoria,
                    descricao = documento.Descricao!,
                    dataAssociada = documento.DataAssociada,
                    dataUpload = documento.DataUpload,
                    caminho = documento.Caminho!,
                    tamanhoArquivo = documento.TamanhoArquivo,
                    tipoArquivo = documento.TipoArquivo!
                }).ToList();
                return CreateResponse<List<DocumentoSaidaDTO>>(true, "Documentos encontrados.", dcoumentosSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar documentos.");
                return CreateResponse<List<DocumentoSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }
    }
}
