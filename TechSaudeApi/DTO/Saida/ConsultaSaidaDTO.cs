using TechSaude.Server.Models;

namespace TechSaude.Server.DTO.Saida
{

    public class ConsultaSaidaDTO
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime DataHora { get; set; }
        public StatusConsultaEnum Status { get; set; }
        public string? Local { get; set; }
        public string? Encaminhamentos { get; set; }
        public PacienteDTO? Paciente { get; set; }
        public MedicoDTO? Medico { get; set; }
    }

    public class PacienteDTO
    {
        public int Id { get; set; }
        public string? NomeCompleto { get; set; }
        public DateTime DataNascimento { get; set; }
        public SexoEnum Sexo { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? Cns { get; set; }
        public string? Convenio { get; set; }
    }

    public class MedicoDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Especialidade { get; set; }
        public string? Localidade { get; set; }
    }

}
