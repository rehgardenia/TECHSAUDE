using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TechSaude.Server.Models
{
    [Table("Medicos")]
    public class Medico
    {
        [Key]
        public int Id { get; set; }
        public string NomeCompleto { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty ;
        public string CRM { get; set; } = string.Empty;
        public string Especialidade { get; set; } = string.Empty ;
        public string Local { get; set; } = string.Empty ;
        public DateTime DataCadastro { get; set; }
        public StatusUserEnum StatusUser { get; set; }

        public List<Consulta>? Consultas { get; set; }=  new List<Consulta>();

    }

}
