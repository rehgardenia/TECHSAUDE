using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSaude.Server.Models
{
    [Table("HistoricoMedicos")]
    public class HistoricoMedico
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int PacienteId { get; set; }
        [ForeignKey("PacienteId")]
        public virtual Paciente? Paciente { get; set; }

        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
        public TipoSanguineoEnum TipoSanguineo { get; set; }

   
        public virtual List<Documento>? Documentos { get; set; } = new List<Documento>();
       
        public virtual List<Alergia>? Alergias { get; set; } = new List<Alergia>();
        public virtual List<Vacina>? Vacinas { get; set; } = new List<Vacina>();
        public virtual List<Doenca>? Doencas { get; set; } = new List<Doenca>();
        

    }

    public enum TipoSanguineoEnum
    {
        A_positivo,
        A_negativo,
        B_positivo,
        B_negativo,
        AB_positivo,
        AB_negativo,
        O_positivo,
        O_negativo,
        NaoInformado
    }
}
