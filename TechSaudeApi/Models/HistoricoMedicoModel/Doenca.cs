using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechSaude.Server.Models
{
    [Table("Doencas")]
    public class Doenca
    {
        // Define a chave primária composta
        [Key, Column(Order = 0)]
        public int HistoricoId { get; set; }

        [Key, Column(Order = 1)]
        public int DoencaId { get; set; }

        public string? Descricao { get; set; }

        public string? Medicamento  { get; set; }
        public string? CDI { get; set; }

        public TipoDoencaEnum Tipo { get; set; }

        public StatusDoencaEnum Status { get; set; }

        // Define o relacionamento com HistoricoMedico
        [ForeignKey("HistoricoId")]
        public HistoricoMedico? Historico { get; set; }
    }
    public enum TipoDoencaEnum
    {
        Crônica,
        Hereditaria,
        Transmissivel
    }

    public enum StatusDoencaEnum
    {
        DiagnosticoInicial,  // Estado quando a doença está sendo diagnosticada
        EmTratamento,        // Estado quando o paciente está recebendo tratamento
        Curado,              // Estado quando a doença foi curada
        Crônico,             // Estado para doenças que são de longo prazo e não curáveis
        EmObservacao,
    }
}
