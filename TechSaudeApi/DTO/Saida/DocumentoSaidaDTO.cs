using TechSaude.Server.Models;

namespace TechSaude.Server.DTO.Saida
{
    public class DocumentoSaidaDTO
    {
        public int historicoId { get; set; }

        public int documentoID { get; set; }
        public string nome { get; set; } = string.Empty;
        public TipoDocumento categoria { get; set; }
        public string descricao { get; set; } = string.Empty;
        public DateTime dataAssociada { get; set; }
        public DateTime dataUpload { get; set; }
        public string caminho { get; set; } = string.Empty;
        public long tamanhoArquivo { get; set; }
        public string tipoArquivo { get; set; } = string.Empty;
    }
}
