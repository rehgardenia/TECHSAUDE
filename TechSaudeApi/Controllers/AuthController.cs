
using Microsoft.AspNetCore.Mvc;
using TechSaude.Server.DTO;
using TechSaude.Server.Models;
using TechSaude.Server.Repository.AuthRepository;

namespace TechSaude.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;


        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        
        [HttpPost("login")]

        public async Task<ServerResponse<ResponseLoginDTO>> Login([FromBody] LoginDTO model)
        {
            return await _authRepository.Login(model);
        }

    }
}
