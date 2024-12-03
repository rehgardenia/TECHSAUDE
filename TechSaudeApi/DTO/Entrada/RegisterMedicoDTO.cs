
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TechSaude.Server.DTO
{

    public class RegisterMedicoDTO
    {
        private static readonly string[] SiglasEstados =
   {
        "AC", "AL", "AM", "AP", "BA", "CE", "DF", "ES", "GO", "MA",
        "MG", "MS", "MT", "PA", "PB", "PE", "PI", "PR", "RJ", "RN",
        "RO", "RR", "RS", "SC", "SE", "SP", "TO"
    };

        [MaxLength(255)]
        [Required(ErrorMessage = "O nome completo deve ser informado!")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "O nome completo deve conter apenas letras e espaços.")]
        public string? NomeCompleto { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "O e-mail deve ser preenchido!")]
        [EmailAddress(ErrorMessage = "O e-mail deve ser válido")]
        public string? Email { get; set; }

        //[Display(Name = "Senha")]
        //[Required(ErrorMessage = "A senha deve ser preenchida!")]
        //[MinLength(6,
        //    ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        //    ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número e um símbolo.")]
        //public string? Senha { get; set; }

        //[Display(Name = "Confirmar Senha")]
        //[Compare("Senha",
        //    ErrorMessage = "Senhas informadas não conferem")]
        //public string? ConfirmarSenha { get; set; }


        [Required(ErrorMessage = "O CRM deve ser preenchido.")]
        [RegularExpression(@"^(?<estado>[A-Z]{2})-\d{5}(/?\d{1})?$", ErrorMessage = "O CRM deve seguir o formato 'XX-12345' ou 'XX-12345/6', onde XX é a sigla do estado.")]
        public string? CRM { get; set; }

        public bool IsValidCRM()
        {
            if (string.IsNullOrEmpty(CRM))
            {
                return false;
            }

            var match = new Regex(@"^(?<estado>[A-Z]{2})-\d{5}(/?\d{1})?$").Match(CRM);
            if (match.Success)
            {
                var estado = match.Groups["estado"].Value;
                return SiglasEstados.Contains(estado);
            }

            return false;
        }
        [Required(ErrorMessage ="Informe o telefone!")]
        [Phone]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Uma especialidade deve ser escolhida.")]
        [RegularExpression(@"^[A-Za-z\s]+$",
            ErrorMessage = "A especialidade deve conter apenas letras e espaços.")]
        public string? Especialidade { get; set; }
        [Required(ErrorMessage =
            "Informe o seu local de trabalho!")]
        public string? LocalTrabalho { get; set; }

        //[Required(ErrorMessage = "O termo deve ser aceito.")]
        //public bool Termo { get; set; }

        //[Required(ErrorMessage = "O compartilhamento deve ser permitido.")]
        //public bool Compartilhamento { get; set; }


    }
}
