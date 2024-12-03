using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSaude.Server.Models
{
    [Table("Alergia")]
    public class Alergia
    {
        [Key, Column(Order = 0)]
        public int HistoricoId { get; set; }
        [Key, Column(Order = 1)]
        public int AlergiaId { get; set; }
        public string? Descricao {  get; set; }
        public string? Medicamento { get; set; }
        public TipoAlergiaEnum TipoAlergia { get; set; }
        public GrauAlergiaEnum Intensidade {  get; set; }

        [ForeignKey("HistoricoId")]
        public HistoricoMedico? Historico { get; set; }
    }

    public enum TipoAlergiaEnum
    {
        Alimentar,
        Medicamento,
        Animal,
        Química,
        Outros
    }
    public enum GrauAlergiaEnum
    {
        Leve,
        Moderada,
        Grave,
        Crítica
    }

}
