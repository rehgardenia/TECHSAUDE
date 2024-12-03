using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.AlergiasRepository
{
    public class AlergiasRepository : IAlergiasRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AlergiasRepository> _logger;

        public AlergiasRepository(AppDbContext context, ILogger<AlergiasRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        // RESPONSE 
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        public async Task<ServerResponse<AlergiaSaidaDTO>> AddAlergia(int idPaciente, AlergiasDTO model)
        {
        
            try
            {
                var historicoMedico = await _context.HistoricosMedicos
                    .Include(h => h.Alergias) 
                    .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                if (historicoMedico == null)
                {
                    return CreateResponse<AlergiaSaidaDTO>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                // Validação básica dos dados da vacina
                if (string.IsNullOrEmpty(model.Descricao))
                {
                    return CreateResponse<AlergiaSaidaDTO>(false, "Informe a alergia!", null);
                }

         
                // Criar a nova vacina
                var alergia = new Alergia
                {
                 Descricao = model.Descricao ,
                 Medicamento = model.Medicamento
                };

                // Adicionar a vacina ao histórico médico
                historicoMedico.Alergias!.Add(alergia);

                // Salvar as mudanças no banco de dados
                await _context.SaveChangesAsync();

                var alergiaSaida = new AlergiaSaidaDTO
                {
                    alergiaId = alergia.AlergiaId,
                    historicoId = alergia.HistoricoId,
                    descricao = alergia.Descricao,
                    medicamento = alergia.Medicamento!
                };

                // Retornar a vacina adicionada com sucesso
                return CreateResponse(true, "Alergia adicionada com sucesso", alergiaSaida);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar alergia.");
                return CreateResponse<AlergiaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<List<AlergiaSaidaDTO>>> GetAlergias(int idPaciente)
        {
            try
            {
                var historicoMedico = await _context.HistoricosMedicos
                .Include(h => h.Alergias) 
                .FirstOrDefaultAsync(h => h.PacienteId == idPaciente);

                if (historicoMedico == null)
                {
                    return CreateResponse<List<AlergiaSaidaDTO>>(false, $"Nenhum histórico médico encontrado para o paciente com ID {idPaciente}.", null);
                }

                var alergiasSaida = historicoMedico.Alergias!.Select(alergia => new AlergiaSaidaDTO
                {
                    alergiaId = alergia.AlergiaId,
                    historicoId = alergia.HistoricoId,
                    descricao = alergia.Descricao!,
                    medicamento = alergia.Medicamento!
                }).ToList();

                // Retorna as vacinas associadas ao histórico médico
                return CreateResponse<List<AlergiaSaidaDTO>>(true, "Alergias encontradas com sucesso", alergiasSaida);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erro ao consultar alergia.");
                return CreateResponse<List<AlergiaSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<AlergiaSaidaDTO>> UpdateAlergia(int idAlergia, AlergiasDTO model)
        {
            try
            {
                var alergia = await _context.Alergias.FirstOrDefaultAsync(v => v.AlergiaId == idAlergia);

                if (alergia == null)
                {
                    return CreateResponse<AlergiaSaidaDTO>(false, $"Nenhuma alergia encontrada com o ID {idAlergia}.", null);
                }


                // Atualiza os dados da vacina
                alergia.Descricao = model.Descricao;
                alergia.Medicamento = model.Medicamento;

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();

                var alergiaSaida = new AlergiaSaidaDTO
                {
                    alergiaId = alergia.AlergiaId,
                    historicoId = alergia.HistoricoId,
                    descricao = alergia.Descricao!,
                    medicamento = alergia.Medicamento!
                };


                return CreateResponse<AlergiaSaidaDTO>(true, "Alergia atualizada com sucesso", alergiaSaida);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Erro ao editar alergia.");
                return CreateResponse<AlergiaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

    }
}

