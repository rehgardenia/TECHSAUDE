using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TechSaude.Server.Models
{
    public class Usuario : IdentityUser<int>
    {
    
        public DateTime  DataCadastro { get; set; } = DateTime.Now;
        public StatusUserEnum Status { get; set; }
        public PerfilUserEnum PerfilUser { get; set; }
        public bool Termo { get; set; }
        public bool Compartilhamento { get; set; }

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatusUserEnum
    {
        Ativo = 1,
        Inativo = 2,
        Bloqueado = 3
    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PerfilUserEnum
    {
        Administrador,
       Paciente
    }


}
