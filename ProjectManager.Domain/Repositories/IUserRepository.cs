using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        Task<User> Create(RegisterUserDTO userDTO);
        Task<User> Update(UpdateUserDTO userDTO, int id);
        Task<User> Delete(int id);
        public int GenerateOTP();
        public Task SendWelcomeEmail(User user);
        public Task SendRecoveryEmail(User user);
        public bool ConfirmOTP(User user, int otp);
        public Task<User> ChangePassword(User user, int otp, string newPassword);
    }
}