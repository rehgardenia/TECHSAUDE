
using System.ComponentModel.DataAnnotations;
using TechSaude.Server.Models;

namespace TechSaude.Server.DTO
{
    public class PerfilPacienteDTO
    {

        public string? NomeCompleto { get; set; }
        public string? DataNascimento { get; set; }
        public SexoEnum Sexo { get; set; }
        public string? Email { get; set; }
        public string? Endereco { get; set; }
        public string? Telefone { get; set; }
        public string? CNS { get; set; }
        public string? Convenio { get; set; }

    }
}
