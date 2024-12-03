using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechSaude.Server.Models
{
    [Table("Vacinas")]
    public class Vacina
    {

        [Key]
        public int VacinaId { get; set; }
        public int HistoricoId { get; set; }
        public string? Nome { get; set; }
        public string? Lote { get; set; }
        public string? UnidadeSaude { get; set; }
        public DateTime Data { get; set; }

        // Define o relacionamento com HistoricoMedico
        [ForeignKey("HistoricoId")]
        public HistoricoMedico? Historico { get; set; }
    }
}
