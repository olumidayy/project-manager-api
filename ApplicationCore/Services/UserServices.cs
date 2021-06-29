using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.ApplicationCore.Entities;
using ProjectManager.ApplicationCore.Entities.DTOs;
using ProjectManager.Data;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManager.ApplicationCore.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        
        User GetByEmail(string email);

        Task<User> Create(UserDTO userDTO);

        Task<User> Update(UpdateUserDTO userDTO, int id);

        Task<User> Delete(int id);
        
    }

    public class UserService : IUserService
    {
        private ApiDbContext _context;
        public UserService(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<User> Create(UserDTO userDTO)
        {
            var newUser = new User()
            {
                Email = userDTO.Email.ToLower(),
                Name = userDTO.Name,
                Hash = BC.HashPassword(userDTO.Password)
            };
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<User> Update(UpdateUserDTO userDTO, int id)
        {
            var userToUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if(userToUpdate != null)
            {
                userToUpdate.Email = userDTO.Email;
                userToUpdate.Name = userDTO.Name;
                await _context.SaveChangesAsync();
            }
            return userToUpdate;
        }

        public async Task<User> Delete(int id)
        {
            var userToDelete = _context.Users.FirstOrDefault(u => u.Id == id);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return userToDelete;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email.ToLower());
        }
    }
}