using System.ComponentModel.DataAnnotations;

namespace TechSaude.Server.DTO
{
    public class AgendamentoConsultaDTO
    {
        [Required]
        public DateTime DataHora { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public int MedicoId { get; set; }

        [MaxLength(500)]
        public string? Local { get; set; }

    }
}
