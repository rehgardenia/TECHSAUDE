using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TechSaude.Server.Models
{
    [Table("Pacientes")]
    public class Paciente : Usuario
    {
       
        public string NomeCompleto { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public SexoEnum Sexo { get; set; }
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string CNS { get; set; } = string.Empty;
        public string Convenio { get; set; } = string.Empty;

        public List<Consulta> Consultas { get; set; } = new List<Consulta>();
        public HistoricoMedico? HistoricosMedicos { get; set; }
    
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SexoEnum
    {
        Feminino,
        Masculino,
        Outro,
        Vazio
    }
}
