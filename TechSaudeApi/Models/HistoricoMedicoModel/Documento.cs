using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TechSaude.Server.Models
{
    [Table("Documentos")]
    public class Documento
    {

        [Key]
        public int DocumentoId { get; set; }

        [Required]
        public string? Nome { get; set; } 

        [Required]
        public TipoDocumento Categoria { get; set; } 

        [Required]
        public string? Descricao { get; set; }

        public DateTime DataAssociada { get; set; } // Data relacionada ao documento, como a data de emissão do exame

        [Required]
        public DateTime DataUpload { get; set; } = DateTime.Now; // Data de quando o arquivo foi enviado ao sistema

        [Required]
        public string? Caminho { get; set; } // Apenas o nome único do arquivo, sem o caminho completo

        [Required]
        public long TamanhoArquivo { get; set; } // Tamanho do arquivo em bytes

        public string? TipoArquivo { get; set; } // Tipo do arquivo (PDF, JPG, etc.)

        public int HistoricoId { get; set; }
        [ForeignKey("HistoricoId")]
        public HistoricoMedico? Historico { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TipoDocumento
    {
        Exames,
        Receitas,
        Outros
    }
}
