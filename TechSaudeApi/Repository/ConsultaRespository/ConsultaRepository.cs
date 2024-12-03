
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AuthRepository;
using TechSaude.Server.Repository.MedicoRepository;

namespace TechSaude.Server.Repository.ConsultaRespository
{
    public class ConsultaRepository: IConsultaRepository
    {
        private readonly AppDbContext _context;
        private readonly IMedicoRepository _medicoRepository;
        private readonly ILogger<ConsultaRepository> _logger;

        public ConsultaRepository(AppDbContext context, ILogger<ConsultaRepository> logger, IMedicoRepository medico)
        {
            _context = context;
            _medicoRepository = medico;
            _logger = logger;
        }
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }

        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultas()
        {
            try
            {
                var consultas = await _context.Consultas
                .Include(c => c.Paciente)
                .Include(c => c.Medico)
                .ToListAsync();

                if (consultas == null || !consultas.Any())
                {
                    return CreateResponse<List<ConsultaSaidaDTO>>(false, "Nenhuma consulta encontrada", null);
                }
                var consultaDTOs = consultas.Select(c => new ConsultaSaidaDTO
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    MedicoId = c.MedicoId,
                    DataHora = c.DataHora,
                    Status = c.Status,
                    Local = c.Local,
                    Encaminhamentos = c.Encaminhamentos!,
                    Paciente = new PacienteDTO
                    {
                        Id = c.Paciente!.Id,
                        NomeCompleto = c.Paciente.NomeCompleto,
                        DataNascimento = c.Paciente.DataNascimento,
                        Sexo = c.Paciente.Sexo,
                        Endereco = c.Paciente.Endereco,
                        Telefone = c.Paciente.Telefone,
                        Cns = c.Paciente.CNS,
                        Convenio = c.Paciente.Convenio
                    },
                    Medico = new MedicoDTO
                    {
                        Id = c.Medico!.Id,
                        Nome = c.Medico.NomeCompleto,
                        Especialidade = c.Medico.Especialidade
                        // Adicione outros campos conforme necessário
                    }
                   
                }).ToList();

                return CreateResponse<List<ConsultaSaidaDTO>>(true, "Consultas  encontradas", consultaDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar consultas.");
                return CreateResponse<List<ConsultaSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null!);

            }
        }

        public async Task<ServerResponse<ConsultaSaidaDTO>> GetConsultaById(int id)
        {
            try
            {
                var consulta = await _context.Consultas
                    .Include(c => c.Paciente)
                    .Include(c => c.Medico)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (consulta == null)
                {
                    return CreateResponse<ConsultaSaidaDTO>(false, "Consulta não encontrada", null);
                }

                var consultaDTO = new ConsultaSaidaDTO
                {
                    Id = consulta.Id,
                    PacienteId = consulta.PacienteId,
                    MedicoId = consulta.MedicoId,
                    DataHora = consulta.DataHora,
                    Status = consulta.Status,
                    Local = consulta.Local,
                    Encaminhamentos = consulta.Encaminhamentos!,
                    Paciente = new PacienteDTO
                    {
                        Id = consulta.Paciente!.Id,
                        NomeCompleto = consulta.Paciente.NomeCompleto,
                        DataNascimento = consulta.Paciente.DataNascimento,
                        Sexo = consulta.Paciente.Sexo,
                        Endereco = consulta.Paciente.Endereco,
                        Telefone = consulta.Paciente.Telefone,
                        Cns = consulta.Paciente.CNS,
                        Convenio = consulta.Paciente.Convenio
                    },
                    Medico = new MedicoDTO
                    {
                        Id = consulta.Medico!.Id,
                        Nome = consulta.Medico.NomeCompleto,
                        Especialidade = consulta.Medico.Especialidade
                        // Adicione outros campos conforme necessário
                    }
                };

                return CreateResponse<ConsultaSaidaDTO>(true, "Consulta encontrada com sucesso.", consultaDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar consultas.");
                return CreateResponse<ConsultaSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null!);
            }
        }
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasByMedico(int medicoId)
        {
            try
            {
                var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == medicoId);
                if (medico == null)
                {
                    return CreateResponse<List<ConsultaSaidaDTO>>(false, "Médico não encontrado", null);
                }

                var consultas = await _context.Consultas
                    .Where(c => c.MedicoId == medicoId)
                    .Include(c => c.Paciente)
                    .Include(c => c.Medico)
                    .ToListAsync();

                var consultaDTOs = consultas.Select(c => new ConsultaSaidaDTO
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    MedicoId = c.MedicoId,
                    DataHora = c.DataHora,
                    Status = c.Status,
                    Local = c.Local,
                    Encaminhamentos = c.Encaminhamentos!,
                    Paciente = new PacienteDTO
                    {
                        Id = c.Paciente!.Id,
                        NomeCompleto = c.Paciente.NomeCompleto,
                        DataNascimento = c.Paciente.DataNascimento,
                        Sexo = c.Paciente.Sexo,
                        Endereco = c.Paciente.Endereco,
                        Telefone = c.Paciente.Telefone,
                        Cns = c.Paciente.CNS,
                        Convenio = c.Paciente.Convenio
                    }
                }).ToList();

                var sucesso = consultas.Any();
                var mensagem = sucesso ? "Consultas encontradas com sucesso." : "Nenhuma consulta encontrada para este médico.";
                return CreateResponse<List<ConsultaSaidaDTO>>(sucesso, mensagem, consultaDTOs);
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao pegar consultas.");
                return CreateResponse<List<ConsultaSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null!);

            }
        }
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasByPaciente(int pacienteId)
        {
            try
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId);
                if (paciente == null)
                {
                    return CreateResponse<List<ConsultaSaidaDTO>>(false, "Paciente não encontrado.", null);

                }
                var consultas = await _context.Consultas
                  .Where(c => c.PacienteId == pacienteId)
                  .Include(c => c.Paciente)
                  .Include(c => c.Medico)
                  .ToListAsync();

                var consultaDTOs = consultas.Select(c => new ConsultaSaidaDTO
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    MedicoId = c.MedicoId,
                    DataHora = c.DataHora,
                    Status = c.Status,
                    Local = c.Local,
                    Paciente = null,
                    Medico = new MedicoDTO
                    {
                        Id = c.Medico!.Id,
                        Nome = c.Medico.NomeCompleto,
                        Especialidade = c.Medico.Especialidade
                    }
                }).ToList();
             

                var sucesso = consultas.Any();
                var mensagem = sucesso ? "Consultas encontradas com sucesso." : "Nenhuma consulta encontrada para este paciente.";
                return CreateResponse(sucesso, mensagem, consultaDTOs);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao acessar consultas do paciente.");
                return CreateResponse<List<ConsultaSaidaDTO>>(false, "Erro ao processar a solicitação. Tente novamente.", null);
            }
        }

      
        public async Task<ServerResponse<Consulta>> AgendarConsulta(int pacienteId, AgendamentoConsultaDTO model)
        {
            try
            {
                // Verificar data
                if (model.DataHora <= DateTime.Now)
                {
                    return CreateResponse<Consulta>(false, "A data e hora da consulta devem ser no futuro.", null);
                }

                // Verificar se o médico existe
                var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == model.MedicoId);
                if (medico == null)
                    return CreateResponse<Consulta>(false, $"Médico com ID {model.MedicoId} não encontrado.", null);

                // Verificar se o paciente existe
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == model.PacienteId);
                if (paciente == null)
                    return CreateResponse<Consulta>(false, $"Paciente com ID {model.PacienteId} não encontrado.", null);

                // Criar uma nova consulta
                var consulta = new Consulta
                {
                    DataHora = model.DataHora,
                    PacienteId = model.PacienteId,
                    MedicoId = model.MedicoId,
                    Local = model.Local,
                    Status = StatusConsultaEnum.Agendada,
                };
   
                // Adicionar a consulta ao contexto
                _context.Consultas.Add(consulta);
                await _context.SaveChangesAsync();
                var message = "Consulta agendada com sucesso!";
                var sucesso = true;

                // Tentar confirmar a consulta após o agendamento
                try
                {
                    var confirmacaoResponse = await _medicoRepository.ConfirmarConsulta(consulta.Id);
                    if (!confirmacaoResponse.Sucesso)
                    {
                        sucesso = false;
                        message += " No entanto, houve um problema ao confirmar a consulta: " + confirmacaoResponse.Message;
                    }
                    else
                    {
                        message += "Além disso, " + confirmacaoResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao confirmar consulta.");
                    message += " No entanto, ocorreu um erro ao confirmar a consulta. Tente novamente ";
                    sucesso = false;
                }

                return CreateResponse<Consulta>(sucesso, message, consulta);

            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao agendar consulta.");
                return CreateResponse<Consulta>(false, "Erro ao processar a solicitação. Tente novamente.", null);
            }
        }
       
        public async Task<ServerResponse<List<ConsultaSaidaDTO>>> GetConsultasConfirmadasByPaciente(int pacienteId)
        {
            try
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId);
                if (paciente == null)
                {
                    return CreateResponse<List<ConsultaSaidaDTO>>(false, "Paciente não encontrado. " + pacienteId , null);
                }

                var consultas = await _context.Consultas
                    .Where(c => c.PacienteId == pacienteId && c.Status == StatusConsultaEnum.Confirmada)
                    .Include(c => c.Medico)
                    .ToListAsync();

                var consultaDTOs = consultas.Select(c => new ConsultaSaidaDTO
                {
                    Id = c.Id,
                    PacienteId = c.PacienteId,
                    MedicoId = c.MedicoId,
                    DataHora = c.DataHora,
                    Status = c.Status,
                    Local = c.Local,
                    Paciente = null,
                    Medico = new MedicoDTO
                    {
                        Id = c.Medico!.Id,
                        Nome = c.Medico.NomeCompleto,
                        Especialidade = c.Medico.Especialidade
                    }
                }).ToList();

                var sucesso = consultas.Any();
                var mensagem = sucesso ? "Consultas confirmadas encontradas com sucesso." : "Nenhuma consulta confirmada encontrada para este paciente.";
                return CreateResponse(sucesso, mensagem, consultaDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao acessar consultas confirmadas do paciente.");
                return CreateResponse<List<ConsultaSaidaDTO>>(false, "Erro ao processar a solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<bool>> DeleteConsulta(int consultaId)
        {

            try
            {

                var consulta = await _context.Consultas.FirstOrDefaultAsync(c => c.Id == consultaId);
                if (consulta == null) {
                    return CreateResponse(false, "Consulta não encontrada.", false);
                }
                

                var result = _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();

          
                if (result == null)
                {
                    return CreateResponse(false, "Erro ao deletar consulta.", false);
                }

                return CreateResponse(true, "Consulta deletada com sucesso.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar consulta.");
                return CreateResponse(false, "Erro ao realizar solicitação. Tente novamente.", false);
            }
        }
    }
}
