
using TechSaude.Server.DTO;
using TechSaude.Server.Models;

namespace TechSaude.Server.Repository.AuthRepository
{
    public interface IAuthRepository
    {
      
        Task<ServerResponse<ResponseLoginDTO>> Login(LoginDTO model);
     
    }
}
