using System.ComponentModel.DataAnnotations;
using TechSaude.Server.Models;

namespace TechSaude.Server.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Campo Obrigatório.")]
        [EmailAddress(ErrorMessage = "O e-mail deve ser válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório.")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número, um símbolo e ter no mínimo 6 caracteres.")]
        public string? Senha { get; set; }

    }

    public class ResponseLoginDTO
    {
        public int Id { get; set; }
        public PerfilUserEnum PerfilUser { get; set; }
        public StatusUserEnum StatusUser { get; set; }

    }
}
