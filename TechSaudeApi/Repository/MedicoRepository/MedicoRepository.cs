
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AuthRepository;

namespace TechSaude.Server.Repository.MedicoRepository
{
    public class MedicoRepository : IMedicoRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MedicoRepository> _logger;
        private readonly IAuthRepository _authRepository;
        //private readonly UserManager<Usuario> _userManager;

        public MedicoRepository(AppDbContext context, ILogger<MedicoRepository> logger, IAuthRepository authRepository, UserManager<Usuario> userManager)
        {
            _context = context;
            _logger = logger;
            _authRepository = authRepository;
            //  _userManager = userManager;
        }

        // RESPONSE
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        // CRUD - CREATE, READ, UPDATE, DELETE
        public async Task<ServerResponse<List<Medico>>> GetMedicos()
        {
            try
            {
                var medicos = await _context.Medicos.AsNoTracking().ToListAsync();
                var sucesso = medicos.Any();
                var mensagem = sucesso ? "Solicitação realizada com sucesso." : "Nenhum médico encontrado.";
                return CreateResponse(sucesso, mensagem, medicos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar médicos.");
                return CreateResponse<List<Medico>>(false, "Erro ao realizar solicitação. Tente novamente.", new List<Medico>());
            }
        }
        public async Task<ServerResponse<Medico>> GetMedicoById(int medicoId)
        {
            try
            {
                var medico = await _context.Medicos.FirstOrDefaultAsync(p => p.Id == medicoId);
                var sucesso = medico != null;
                var mensagem = sucesso ? "Médico encontrado com sucesso." : "Médico não encontrado.";
                return CreateResponse<Medico>(sucesso, mensagem, medico!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar médico pelo ID.");
                return CreateResponse<Medico>(false, "Erro ao realizar solicitação. Tente novamente.", null!);
            }
        }
        public async Task<ServerResponse<Medico>> CreateMedico(RegisterMedicoDTO model)
        {
            try
            {
                //if (!model.Termo || !model.Compartilhamento)
                //    return CreateResponse<MedicoModel>(false, "O termo de compromisso e de compartilhamento são obrigatórios.", null!);

                //var existingUser = await _userManager.FindByEmailAsync(model.Email!);
                var existingUser = await _context.Medicos.FirstOrDefaultAsync(m => m.Email == model.Email);
                if (existingUser != null)
                    return CreateResponse<Medico>(false, "Já existe um usuário cadastrado com este e-mail.", null!);

                if (!model.IsValidCRM())
                    return CreateResponse<Medico>(false, "O CRM informado é inválido.", null!);


                var user = new Medico
                {
                    Email = model.Email!,
                    NomeCompleto = model.NomeCompleto!,
                    CRM = model.CRM!,
                    Especialidade = model.Especialidade!,
                    Local = model.LocalTrabalho!,
                    DataCadastro = DateTime.Now,
                    StatusUser = StatusUserEnum.Ativo,
                    Telefone = model.Telefone!
                    //Termo = model.Termo,
                    //Compartilhamento = model.Compartilhamento,
                };

                //var result = await _userManager.CreateAsync(user, model.Senha!);
                await _context.Medicos.AddAsync(user);
                await _context.SaveChangesAsync();
                return CreateResponse<Medico>(true, "Medico adicionado com sucesso.", user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar médico.");
                return CreateResponse<Medico>(false, "Erro ao processar a solicitação. Tente novamente.", null!);
            }
        }
        public async Task<ServerResponse<Medico>> UpdateMedico(int medicoId, Medico model)
        {
            try
            {
                var medicoExistente = await _context.Medicos.FindAsync(medicoId);
                if (medicoExistente == null)
                    return CreateResponse<Medico>(false, "Médico não encontrado.", null!);

                medicoExistente.NomeCompleto = model.NomeCompleto;
                medicoExistente.Email = model.Email;
                medicoExistente.Telefone = model.Telefone;
                medicoExistente.Especialidade = model.Especialidade;
                medicoExistente.Local = model.Local;

                await _context.SaveChangesAsync();
                return CreateResponse(true, "Médico atualizado com sucesso.", medicoExistente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar dados do médico.");
                return CreateResponse<Medico>(false, "Erro ao realizar solicitação. Tente novamente.", null!);
            }
        }
        public async Task<ServerResponse<bool>> DeleteMedico(int medicoId)
        {
            try
            {
                var medicoResponse = await GetMedicoById(medicoId);
                if (!medicoResponse.Sucesso)
                {
                    return CreateResponse(false, medicoResponse.Message, false);
                }

                //                var user = await _userManager.FindByIdAsync(medicoId.ToString());
                var user = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == medicoId);
                if (user == null)
                {
                    return CreateResponse(false, "Usuário associado ao médico não encontrado.", false);
                }

                var result = _context.Medicos.Remove(medicoResponse.Data!);
                await _context.SaveChangesAsync();

                //              var result = await _userManager.DeleteAsync(user);
                if (result == null)
                {
                    return CreateResponse(false, "Erro ao deletar usuário.", false);
                }

                return CreateResponse(true, "Médico deletado com sucesso.", true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar médico.");
                return CreateResponse(false, "Erro ao realizar solicitação. Tente novamente.", false);
            }
        }
        // MÉTODOS SECUNDÁRIOS
        public async Task<ServerResponse<Consulta>> ConfirmarConsulta(int consultaId)
        {
            try
            {
                var consultaAgendada = await _context.Consultas
                      .Include(c => c.Medico)
                      .Include(c => c.Paciente)
                      .FirstOrDefaultAsync(c => c.Id == consultaId);

                if (consultaAgendada == null)
                    return CreateResponse<Consulta>(false, "Consulta não encontrada.", null!);

                if (consultaAgendada.Medico == null)
                    return CreateResponse<Consulta>(false, "Médico não encontrado.", null!);

                if (consultaAgendada.Paciente == null)
                    return CreateResponse<Consulta>(false, "Paciente não encontrado.", null!);

                if (consultaAgendada.Status != StatusConsultaEnum.Agendada)
                    return CreateResponse<Consulta>(false, "Esta consulta não está agendada.", null!);

                if (consultaAgendada.Status == StatusConsultaEnum.Confirmada)
                    return CreateResponse<Consulta>(false, "A consulta já está confirmada.", null!);

                if (consultaAgendada.Status == StatusConsultaEnum.Cancelada)
                    return CreateResponse<Consulta>(false, "A consulta foi cancelada.", null!);

                if (DateTime.Now > consultaAgendada.DataHora.Date)
                    return CreateResponse<Consulta>(false, "A consulta está fora do horário permitido para confirmação.", null!);

                // Verificar se a consulta está fora do dia permitido
                if (IsWeekend(consultaAgendada.DataHora))
                {
                    consultaAgendada.Status = StatusConsultaEnum.Cancelada;
                    await _context.SaveChangesAsync();
                    return CreateResponse(false, "Consulta não pode ser agendada nos fins de semana.", consultaAgendada);
                }

                // Verificar se a consulta está fora do horário permitido
                if (!IsWithinAllowedHours(consultaAgendada.DataHora))
                {
                    consultaAgendada.Status = StatusConsultaEnum.Cancelada;
                    await _context.SaveChangesAsync();
                    return CreateResponse(false, "A consulta deve ser agendada entre 08:00 e 16:00.", consultaAgendada);
                }
                if (consultaAgendada.Local != consultaAgendada.Medico.Local)
                {
                    consultaAgendada.Status = StatusConsultaEnum.Cancelada;
                    await _context.SaveChangesAsync();
                    return CreateResponse(false, "Local da consulta inválido.", consultaAgendada);
                }

                var duracaoConsulta = 30; // Minutos, ajustar conforme necessário

                var intervaloInicio = consultaAgendada.DataHora.AddMinutes(-duracaoConsulta);
                var intervaloFim = consultaAgendada.DataHora.AddMinutes(duracaoConsulta);

                var consultasNoIntervalo = await _context.Consultas
                    .Where(c => c.MedicoId == consultaAgendada.MedicoId
                                && c.DataHora >= intervaloInicio
                                && c.DataHora <= intervaloFim
                                && c.Id != consultaId
                                && c.Status != StatusConsultaEnum.Cancelada)
                    .ToListAsync();

                if (consultasNoIntervalo.Any())
                {
                    return CreateResponse(false, "Já existe uma consulta agendada para este horário.", consultaAgendada);
                }

                // Confirmar a consulta
                consultaAgendada.Status = StatusConsultaEnum.Confirmada;
                await _context.SaveChangesAsync();
                return CreateResponse(true, "Consulta confirmada com sucesso.", consultaAgendada);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao confirmar consulta.");
                return CreateResponse<Consulta>(false, "Erro ao realizar solicitação. Tente novamente.", null!);

            }
        }

        public async Task<ServerResponse<List<MedicoDTO>>> FiltrarMedicos(string especialidade, string localidade)
        {
            try
            {
                var medicos = await _context.Medicos
                    .Where(m => m.Especialidade == especialidade && m.Local == localidade)
                    .ToListAsync();

                var medicoDTOs = medicos.Select(m => new MedicoDTO
                {
                    Id = m.Id,
                    Nome = m.NomeCompleto,
                    Especialidade = m.Especialidade,
                    Localidade = m.Local
                }).ToList();

                var sucesso = medicos.Any();
                var mensagem = sucesso ? "Médicos encontrados com sucesso." : "Nenhum médico encontrado com os filtros fornecidos.";
                return CreateResponse<List<MedicoDTO>>(sucesso, mensagem, medicoDTOs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao filtrar médicos.");
                return CreateResponse<List<MedicoDTO>>(false, "Erro ao processar a solicitação. Tente novamente.", null);
            }
        }
        public async Task<ServerResponse<List<EspecialidadeDTO>>> GetEspecialidades()
        {
            try
            {
                var especialidades = await _context.Medicos
                    .Select(m => m.Especialidade)
                    .Distinct()
                    .ToListAsync();

                var especialidadeDTOs = especialidades.Select(e => new EspecialidadeDTO
                {
                    Nome = e
                }).ToList();

                return new ServerResponse<List<EspecialidadeDTO>>
                {
                    Sucesso = true,
                    Message = "Especialidades encontradas com sucesso.",
                    Data = especialidadeDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar especialidades.");
                return CreateResponse<List<EspecialidadeDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

        public async Task<ServerResponse<List<LocalidadeDTO>>> GetLocalidades( string especialidade)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(especialidade))
                {
                    return CreateResponse<List<LocalidadeDTO>>(false, "Especialidade não pode ser nula ou vazia.", null);
                }

                var localidades = await _context.Medicos
                    .Where(m => m.Especialidade.ToLower() == especialidade.ToLower()) // Filtra pela especialidade (agora como string)
                    .Select(m => m.Local) // Seleciona as localidades
                    .ToListAsync();

                var localidadeDTOs = localidades.Select(l => new LocalidadeDTO
                {
                    Nome = l
                }).ToList();

                return new ServerResponse<List<LocalidadeDTO>>
                {
                    Sucesso = true,
                    Message = "Localidades encontradas com sucesso.",
                    Data = localidadeDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar localidade.");
                return CreateResponse<List<LocalidadeDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", null);

            }
        }

        // FUNÇÕES
        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
        private bool IsWithinAllowedHours(DateTime date)
        {
            TimeSpan horarioConsulta = date.TimeOfDay;
            TimeSpan horarioAbertura = new(8, 0, 0);
            TimeSpan horarioFechamento = new(16, 0, 0);

            return horarioConsulta >= horarioAbertura && horarioConsulta <= horarioFechamento;
        }

    }
}