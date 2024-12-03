using Microsoft.EntityFrameworkCore;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.VacinaRepository
{
    public class VacinasRepository : IVacinaRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<VacinasRepository> _logger;

        public VacinasRepository(AppDbContext context, ILogger<VacinasRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // RESPONSE 
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        public async Task<ServerResponse<VacinaSaidaDTO>> AddVacina(int idPaciente, VacinaDTO model)
        {
        
            try
            {
                
                var historicoMedico = await _context.HistoricosMedicos
                    .Include(h => h.Vacinas) 
                    .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                if (historicoMedico == null)
                {
                    return CreateResponse<VacinaSaidaDTO>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                // Validação básica dos dados da vacina
                if (string.IsNullOrEmpty(model.Nome) || model.Data == null)
                {
                    return CreateResponse<VacinaSaidaDTO>(false, "Dados da vacina inválidos.", null);
                }

                if (!DateTime.TryParse(model.Data, out DateTime datavacina))
                    return CreateResponse<VacinaSaidaDTO>(false, "Data  Inválida.", null);

                // Criar a nova vacina
                var vacina = new Vacina
                {
                    Nome = model.Nome,
                    Lote = model.Lote,
                    UnidadeSaude = model.UnidadeSaude,
                    Data = datavacina,
                };

                // Adicionar a vacina ao histórico médico
                historicoMedico.Vacinas!.Add(vacina);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                var vacinaSaida = new VacinaSaidaDTO
                {
                    historicoId = historicoMedico.Id, // Supondo que você tenha o ID do histórico
                    vacinaId = vacina.VacinaId, // Supondo que a vacina tenha uma propriedade ID
                    nome = vacina.Nome,
                    unidadeSaude = vacina.UnidadeSaude!,
                    lote = vacina.Lote!,
                    data = vacina.Data
                };

                // Retornar a vacina adicionada com sucesso
                return CreateResponse<VacinaSaidaDTO>(true, "Vacina adicionada com sucesso", vacinaSaida);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar vacina.");
                return CreateResponse<VacinaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<List<VacinaSaidaDTO>>> GetVacinas(int idPaciente)
        {
            try
            {
                var historicoMedico = await _context.HistoricosMedicos
                    .Include(h => h.Vacinas) // Inclui as vacinas associadas
                    .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                if (historicoMedico == null)
                {
                    return CreateResponse<List<VacinaSaidaDTO>>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                // Mapeia as vacinas associadas ao histórico médico para a lista de DTOs
                var vacinasSaida = historicoMedico.Vacinas?.Select(v => new VacinaSaidaDTO
                {
                    historicoId = historicoMedico.Id, // Supondo que você tenha essa propriedade
                    vacinaId = v.VacinaId,
                    nome = v.Nome!,
                    unidadeSaude = v.UnidadeSaude!,
                    lote = v.Lote!,
                    data = v.Data
                }).ToList();

                return CreateResponse<List<VacinaSaidaDTO>>(true, "Vacinas encontradas com sucesso", vacinasSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar vacina.");
                return CreateResponse<List<VacinaSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }


        public async Task<ServerResponse<VacinaSaidaDTO>> UpdateVacina(int idVacina, VacinaDTO model)
        {
            try
            {
                var vacina = await _context.Vacinas.FirstOrDefaultAsync(v => v.VacinaId == idVacina);

                if (vacina == null)
                {
                    return CreateResponse<VacinaSaidaDTO>(false, $"Nenhuma vacina encontrada com o ID {idVacina}.", null);
                }

                if (!DateTime.TryParse(model.Data, out DateTime datavacina))
                    return CreateResponse<VacinaSaidaDTO>(false, "Data inválida.", null);

                // Atualiza os dados da vacina
                vacina.Nome = model.Nome;
                vacina.Lote = model.Lote;
                vacina.UnidadeSaude = model.UnidadeSaude;
                vacina.Data = datavacina;

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                // Mapeia a vacina atualizada para o DTO
                var vacinaSaida = new VacinaSaidaDTO
                {
                    historicoId = vacina.HistoricoId, // Supondo que você tenha essa propriedade
                    vacinaId = vacina.VacinaId,
                    nome = vacina.Nome!,
                    unidadeSaude = vacina.UnidadeSaude!,
                    lote = vacina.Lote!,
                    data = vacina.Data
                };

                return CreateResponse<VacinaSaidaDTO>(true, "Vacina atualizada com sucesso", vacinaSaida);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao editar vacina.");
                return CreateResponse<VacinaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

    }
}

