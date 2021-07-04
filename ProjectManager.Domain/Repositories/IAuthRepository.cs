using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories
{
    public interface IAuthenticationRepository
    {
        User Authenticate(UserDTO userDTO);
        string GenerateJwtToken(User user);
    }
}