
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.DTO.Saida;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AuthRepository;
using TechSaude.Server.Repository.MedicoRepository;

namespace TechSaude.Server.Repository.PacienteRepository
{
    public class PacienteRepository : IPacienteRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PacienteRepository> _logger;

        private readonly IAuthRepository _authRepository;
        private readonly UserManager<Usuario> _userManager;

        private readonly IMedicoRepository _medicoRepository;

        public PacienteRepository(AppDbContext context, IMedicoRepository medicoRepository, ILogger<PacienteRepository> logger, IAuthRepository authRepository, UserManager<Usuario> userManager)
        {
            _context = context;
            _medicoRepository = medicoRepository;
            _logger = logger;
            _authRepository = authRepository;
            _userManager = userManager;
        }
        // RESPONSE 
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T? data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data };
        }
        // CRUD - CREATE , READ, UPDATE, DELETE
        public async Task<ServerResponse<List<UsuarioSaidaDTO>>> GetPacientes()
        {
            try
            {
                //var pacientes = await _context.Pacientes.Where(p => p.Status == StatusUsuarioEnum.Ativo).ToListAsync();
                var pacientes = await _context.Pacientes.Select(c => new UsuarioSaidaDTO 
                {
                   Id = c.Id,
                    userName = c.UserName!,
                    email = c.Email!,
                    nomeCompleto = c.NomeCompleto,
                    dataNascimento = c.DataNascimento,
                    perfilUser = c.PerfilUser,
                    sexo = c.Sexo,
                    endereco = c.Endereco,
                    telefone = c.Telefone,
                    cns = c.CNS,
                    convenio = c.Convenio,
                    termo = c.Termo,
                    compartilhamento = c.Compartilhamento
                }).ToListAsync();


                var sucesso = pacientes.Any();
                var mensagem = sucesso ? "Solicitação realizada com sucesso." : "Nenhum paciente encontrado.";
                return CreateResponse(sucesso, mensagem, pacientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar paciente.");
                return CreateResponse<List<UsuarioSaidaDTO>>(false, "Erro ao realizar solicitação. Tente novamente.", new List<UsuarioSaidaDTO>());
            }
        }
        public async Task<ServerResponse<UsuarioSaidaDTO>> GetPacienteById(int pacienteId)
        {
            try
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId);

                if (paciente == null)
                {
                    return CreateResponse<UsuarioSaidaDTO>(false, "Paciente não encontrado.", null);
                }
                var pacienteSaida = new UsuarioSaidaDTO
                {
                    Id = paciente.Id,
                    userName = paciente.UserName!,
                    email = paciente.Email!,
                    nomeCompleto = paciente.NomeCompleto,
                    dataNascimento = paciente.DataNascimento,
                    perfilUser = paciente.PerfilUser,
                    sexo = paciente.Sexo,
                    endereco = paciente.Endereco,
                    telefone = paciente.Telefone,
                    cns = paciente.CNS,
                    convenio = paciente.Convenio,
                    termo = paciente.Termo,
                    compartilhamento = paciente.Compartilhamento
                };
                return CreateResponse<UsuarioSaidaDTO>(true, "Paciente  encontrado com sucesso.", pacienteSaida);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar paciente.");
                return CreateResponse<UsuarioSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null!);
            }
        }
        public async Task<ServerResponse<UsuarioSaidaDTO>> CreatePaciente(RegisterPacienteDTO model)
        {
            try
            {
                // Validação dos termos de compromisso
                if (!model.Termo || !model.Compartilhamento)
                    return CreateResponse<UsuarioSaidaDTO>(false, "O termo de compromisso e de compartilhamento são obrigatórios.", null);

                // Validação do e-mail
                var existingUser = await _userManager.FindByEmailAsync(model.Email!);
                if (existingUser != null)
                    return CreateResponse<UsuarioSaidaDTO>(false, "Já existe um usuário cadastrado com este e-mail.", null);

                // Validação do CNS
                var existingCns = await _context.Pacientes.FirstOrDefaultAsync(p => p.CNS == model.CNS);
                if (existingCns != null)
                    return CreateResponse<UsuarioSaidaDTO>(false, "Já existe um usuário cadastrado com este CNS.", null);

                // Validação da data de nascimento
                if (!DateTime.TryParse(model.DataNascimento, out DateTime dataNascimento))
                    return CreateResponse<UsuarioSaidaDTO>(false, "Data de nascimento inválida.", null);


                // Criação do paciente
                var paciente = new Paciente
                {
                    UserName = model.Email,
                    Status = StatusUserEnum.Ativo,
                    DataCadastro = DateTime.Now,
                    Email = model.Email,
                    Termo = model.Termo,
                    Compartilhamento = model.Compartilhamento,
                    Sexo = SexoEnum.Vazio,
                    PerfilUser = PerfilUserEnum.Paciente,
                    NomeCompleto = model.NomeCompleto!,
                    CNS = model.CNS!,
                    DataNascimento = dataNascimento,
                    Telefone = model.telefone!
                };

                // Criação do usuário
                var result = await _userManager.CreateAsync(paciente, model.Senha!);
                if (result.Succeeded)
                {
                    var historicoMedico = new HistoricoMedico
                    {
                        Paciente = paciente,
                        DataRegistro = DateTime.UtcNow,
                        TipoSanguineo = TipoSanguineoEnum.NaoInformado
                    };
                    await _context.HistoricosMedicos.AddAsync(historicoMedico);
                    await _context.SaveChangesAsync();

                    var pacienteSaidaDTO = new UsuarioSaidaDTO
                    {
                        Id = paciente.Id,
                        userName = paciente.UserName!,
                        email = paciente.Email!,
                        nomeCompleto = paciente.NomeCompleto,
                        dataNascimento = paciente.DataNascimento,
                        perfilUser = paciente.PerfilUser,
                        sexo = paciente.Sexo,
                        endereco = paciente.Endereco,
                        telefone = paciente.Telefone,
                        cns = paciente.CNS,
                        convenio = paciente.Convenio,
                        termo = paciente.Termo,
                        compartilhamento = paciente.Compartilhamento
                    };


                    return CreateResponse<UsuarioSaidaDTO>(true, "Paciente registrado com sucesso!", pacienteSaidaDTO);
                
                }
                else
                {
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    return CreateResponse<UsuarioSaidaDTO>(false, $"Erro ao registrar paciente: {errorMessage}", null);
                }
            }
            catch (Exception ex)
            {
                // Loga o erro com detalhes
                _logger.LogError(ex, "Erro ao registrar paciente.");
                return CreateResponse<UsuarioSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
      
        }
        public async Task<ServerResponse<bool>> DeletePaciente(int pacienteId)
        {
            try
            {
                var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId);
                if (paciente == null)
                    return CreateResponse(false, "Paciente não encontrado.", false);
                
                var user = await _userManager.FindByIdAsync(paciente.Id.ToString());
                if (user == null)
                    return CreateResponse(false, "Usuário não encontrado", false);

                var value = _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                var result = await _userManager.DeleteAsync(user);

                var sucesso = result.Succeeded;
                var mensagem = sucesso ? "Paciente deletado com sucesso!" : "Erro ao deletar paciente.";
                return CreateResponse(sucesso, mensagem, sucesso);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar paciente.");
                return CreateResponse(false, "Erro ao realizar solicitação. Tente novamente.", false);
            }
        }
        public async Task<ServerResponse<UsuarioSaidaDTO>> UpdatePaciente(int pacienteId, PerfilPacienteDTO model)
        {
            try
            {
                var pacienteExistente = await _context.Pacientes.FindAsync(pacienteId);

                if (pacienteExistente == null)
                    return CreateResponse<UsuarioSaidaDTO>(false, "Paciente não encontrado.", null!);

            
                pacienteExistente.NomeCompleto = model.NomeCompleto!;
                pacienteExistente.Email = model.Email;
                pacienteExistente.DataNascimento = DateTime.Parse(model.DataNascimento!.ToString());
                pacienteExistente.Sexo = model.Sexo;
                pacienteExistente.Endereco = model.Endereco!;
                pacienteExistente.Telefone = model.Telefone!;
                pacienteExistente.CNS = model.CNS!;
                pacienteExistente.Convenio = model.Convenio!;
              
                await _context.SaveChangesAsync();

                var pacienteSaidaDTO = new UsuarioSaidaDTO
                {
                    Id = pacienteExistente.Id,
                    userName = pacienteExistente.UserName!,
                    email = pacienteExistente.Email!,
                    nomeCompleto = pacienteExistente.NomeCompleto,
                    dataNascimento = pacienteExistente.DataNascimento,
                    perfilUser = pacienteExistente.PerfilUser,
                    sexo = pacienteExistente.Sexo,
                    endereco = pacienteExistente.Endereco,
                    telefone = pacienteExistente.Telefone,
                    cns = pacienteExistente.CNS,
                    convenio = pacienteExistente.Convenio,
                    termo = pacienteExistente.Termo,
                    compartilhamento = pacienteExistente.Compartilhamento
                };

                return CreateResponse<UsuarioSaidaDTO>(true, "Paciente atualizado com sucesso.", pacienteSaidaDTO); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar dados do paciente.");
                return CreateResponse<UsuarioSaidaDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null);
            }
        }

       
    }

}
