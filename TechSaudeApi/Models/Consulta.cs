using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TechSaude.Server.Models
{
    [Table("Consultas")]
    public class Consulta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PacienteId { get; set; }
        [Required]
        public int MedicoId { get;set; }

        public DateTime DataHora { get; set; }
        public StatusConsultaEnum Status { get; set; }
        public string? Local {  get; set; }  
        public string? Motivo { get; set; }

        public string? Encaminhamentos { get; set; } // Arquivo

        // Relacionamentos
        [ForeignKey(nameof(PacienteId))]
        public virtual Paciente? Paciente { get; set; }

        [ForeignKey(nameof(MedicoId))]
        public virtual Medico? Medico { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusConsultaEnum
    {
        Agendada,
        Confirmada,
        Cancelada,
        Realizada
    }
}
