using TechSaude.Server.Models;

namespace TechSaude.Server.DTO
{
    public class UsuarioSaidaDTO
    {
        public int Id { get; set; }
        public string userName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty ;
 
        public string nomeCompleto { get; set; } = string.Empty;
        public DateTime dataNascimento { get; set; }

        public PerfilUserEnum perfilUser { get; set; }
        public SexoEnum sexo { get; set; }
        public string endereco { get; set; } = string.Empty;

        public string telefone { get; set; } = string.Empty;

        public string cns { get; set; } = string.Empty;

        public string convenio { get; set; } = string.Empty;
        public bool termo { get; set; }
        public bool compartilhamento { get; set; }

  

    }
}