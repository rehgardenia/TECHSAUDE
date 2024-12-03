
using System.ComponentModel.DataAnnotations;

namespace TechSaude.Server.DTO
{
    public class RegisterPacienteDTO
    {
        [MaxLength(255)]
        [Required(ErrorMessage = "O nome completo deve ser informado!")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome completo deve conter apenas letras e espaços.")]
        public string? NomeCompleto { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail deve ser preenchido!")]
        [EmailAddress(ErrorMessage = "O e-mail deve ser válido")]
        public string? Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha deve ser preenchida!")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número, um símbolo e ter no mínimo 6 caracteres.")]
        public string? Senha { get; set; }

        [Display(Name = "Confirmar Senha")]
        [Compare("Senha", ErrorMessage = "Senhas informadas não conferem")]
        public string? ConfirmarSenha { get; set; }
        public string? DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo deve ser preenchido!")]
        [RegularExpression(@"^\d{15}$", ErrorMessage = "O CNS deve conter exatamente 15 dígitos numéricos.")]
        public string? CNS { get; set; } // Campo específico de Paciente

        [Required(ErrorMessage = "O campo deve ser preenchido!")]
        public string? telefone { get; set; }
        
        [Required(ErrorMessage = "O termo deve ser aceito.")]
        public bool Termo { get; set; }
        [Required(ErrorMessage = "O compartilhamento deve ser permitido.")]
        public bool Compartilhamento { get; set; }

    }
}
