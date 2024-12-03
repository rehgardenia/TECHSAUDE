
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechSaude.Server.Data;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ILogger<AuthRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;


        public AuthRepository(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ILogger<AuthRepository> logger, IConfiguration configuration, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            _configuration = configuration;

        }
        // RESPONSE
        private ServerResponse<T> CreateResponse<T>(bool sucesso, string message, T data, string token = "")
        {
            return new ServerResponse<T> { Sucesso = sucesso, Message = message, Data = data, Token = token };
        }
        // MÉTODOS PRINCIPAIS
        public async Task<ServerResponse<ResponseLoginDTO>> Login(LoginDTO model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email!);
                if (user == null)
                    return CreateResponse<ResponseLoginDTO>(false, "Usuário não encontrado", null!, null!);

                if (!IsValidPassword(model.Senha!))
                    return CreateResponse<ResponseLoginDTO>(false, "A senha deve conter pelo menos um número, uma letra maiúscula e um símbolo especial.", null!);


                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Senha!, lockoutOnFailure: false);
                if (!result.Succeeded)
                    return CreateResponse<ResponseLoginDTO>(false, "Senha incorreta", null!);

                var token = GenerateJwtToken(user);

                var response = new ResponseLoginDTO
                {
                    Id = user.Id,
                    PerfilUser = user.PerfilUser,
                    StatusUser = user.Status
                };
                return CreateResponse<ResponseLoginDTO>(true, "Usuário logado com sucesso", response, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login.");
                return CreateResponse<ResponseLoginDTO>(false, "Erro ao realizar solicitação. Tente novamente.", null!);
            }
        }
        // MÉTODOS SECUNDÁRIOS
        private string GenerateJwtToken(Usuario user)
        {
            var jwtSecret = _configuration.GetSection("Jwt").GetRequiredSection("SecretKey").Value!;
            var jwtExpiryMin = int.Parse(_configuration.GetSection("Jwt").GetRequiredSection("ExpiryMinutes").Value!);
            var jwtAudience = _configuration.GetSection("Jwt").GetRequiredSection("Audience").Value!;
            var jwtIssuer = _configuration.GetSection("Jwt").GetRequiredSection("Issuer").Value!;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!)
        }),
                Expires = DateTime.UtcNow.AddMinutes(jwtExpiryMin),
                Audience = jwtAudience, // Use a audiência do appsettings.json
                Issuer = jwtIssuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool IsValidPassword(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                return false;
            }

            bool hasUpperCase = senha.Any(char.IsUpper);
            bool hasNumber = senha.Any(char.IsDigit);
            bool hasSymbol = senha.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpperCase && hasNumber && hasSymbol;
        }


    }
}
