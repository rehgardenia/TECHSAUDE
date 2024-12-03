using Microsoft.EntityFrameworkCore;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.DoencasRepository
{
    public class DoencasRepository : IDoencasRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DoencasRepository> _logger;

        public DoencasRepository(AppDbContext context, ILogger<DoencasRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // RESPONSE 
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        public async Task<ServerResponse<DoencaSaidaDTO>> AddDoenca(int idPaciente, DoencasDTO model)
        {
            try
            {
                // Obtém o histórico médico do paciente
                var historicoMedico = await _context.HistoricosMedicos
                    .Include(h => h.Doencas) // Inclui as doenças associadas
                    .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                // Verifica se o histórico médico existe
                if (historicoMedico == null)
                {
                    return CreateResponse<DoencaSaidaDTO>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                // Validação básica dos dados da doença
                if (string.IsNullOrEmpty(model.Descricao) || string.IsNullOrEmpty(model.Medicamento))
                {
                    return CreateResponse<DoencaSaidaDTO>(false, "Dados da doença inválidos.", null);
                }

                // Criar a nova doença
                var doenca = new Doenca
                {
                    Descricao = model.Descricao,
                    Medicamento = model.Medicamento
                };

                // Adicionar a doença ao histórico médico
                historicoMedico.Doencas!.Add(doenca);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Criar o DTO para retorno
                var doencaSaidaDTO = new DoencaSaidaDTO
                {
                    historicoId = historicoMedico.Id, // Supondo que a entidade HistoricoMedico tenha uma propriedade Id
                    doencaId = doenca.DoencaId, // Supondo que Doenca tenha uma propriedade DoencaId
                    descricao = doenca.Descricao,
                    medicamento = doenca.Medicamento
                };

                // Retornar a doença adicionada com sucesso
                return CreateResponse<DoencaSaidaDTO>(true, "Doença adicionada com sucesso", doencaSaidaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar doença.");
                return CreateResponse<DoencaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }


        public async Task<ServerResponse<List<DoencaSaidaDTO>>> GetDoencas(int idPaciente)
        {
            try
            {
                // Obtém o histórico médico do paciente, incluindo as doenças
                var historicoMedico = await _context.HistoricosMedicos
                    .Include(h => h.Doencas) // Inclui as doenças associadas
                    .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                // Verifica se o histórico médico existe
                if (historicoMedico == null)
                {
                    return CreateResponse<List<DoencaSaidaDTO>>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                // Mapeia as doenças para DTOs
                var doencasSaidaDTO = historicoMedico.Doencas!.Select(d => new DoencaSaidaDTO
                {
                    historicoId = historicoMedico.Id, // Supondo que a entidade HistoricoMedico tenha uma propriedade Id
                    doencaId = d.DoencaId, // Supondo que Doenca tenha uma propriedade DoencaId
                    descricao = d.Descricao!,
                    medicamento = d.Medicamento!
                }).ToList();

                // Retorna as doenças encontradas com sucesso
                return CreateResponse<List<DoencaSaidaDTO>>(true, "Doenças encontradas com sucesso", doencasSaidaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar doenças.");
                return CreateResponse<List<DoencaSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<DoencaSaidaDTO>> UpdateDoenca(int id, DoencasDTO model)
        {
            try
            {
                // Verificação básica de validação do modelo
                if (string.IsNullOrEmpty(model.Descricao) || string.IsNullOrEmpty(model.Medicamento))
                {
                    return CreateResponse<DoencaSaidaDTO>(false, "Dados inválidos. Descrição e medicamento são obrigatórios.", null);
                }

                // Busca a doença pelo ID
                var doenca = await _context.Doencas.FirstOrDefaultAsync(d => d.DoencaId == id);

                if (doenca == null)
                {
                    return CreateResponse<DoencaSaidaDTO>(false, $"Nenhuma doença encontrada com o ID {id}.", null);
                }

                // Atualiza os dados da doença
                doenca.Descricao = model.Descricao;
                doenca.Medicamento = model.Medicamento;

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Cria o DTO para resposta
                var doencaSaidaDTO = new DoencaSaidaDTO
                {
                    historicoId = doenca.HistoricoId, // Supondo que a entidade Doenca tenha uma propriedade HistoricoId
                    doencaId = doenca.DoencaId,
                    descricao = doenca.Descricao,
                    medicamento = doenca.Medicamento
                };

                return CreateResponse<DoencaSaidaDTO>(true, "Doença atualizada com sucesso", doencaSaidaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar doença.");
                return CreateResponse<DoencaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }


    }
}

