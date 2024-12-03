using TechSaude.Server.Models;

namespace TechSaude.Server.DTO
{
    public class FileDTO
    {
        public TipoDocumento categoria { get; set; }
        public int pacienteId { get; set; }
        public string? descricao { get; set; }
        public string? dataAssociada { get; set; }
        public IFormFile? file { get; set; }
    }
}
