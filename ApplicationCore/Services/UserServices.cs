using System;
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
        Task<User> Create(RegisterUserDTO userDTO);
        Task<User> Update(UpdateUserDTO userDTO, int id);
        Task<User> Delete(int id);
        public int GenerateOTP();
        public Task SendWelcomeEmail(User user);
        public Task SendRecoveryEmail(User user);
        public bool ConfirmOTP(User user, int otp);
        public Task<User> ChangePassword(User user, int otp, string newPassword);
        
    }

    public class UserService : IUserService
    {
        private ApiDbContext _context;
        private IEmailService _emailService;
        public UserService(ApiDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<User> Create(RegisterUserDTO userDTO)
        {
            var newUser = new User()
            {
                Email = userDTO.Email.ToLower(),
                Name = userDTO.Name,
                Hash = BC.HashPassword(userDTO.Password),
                Otp = GenerateOTP()
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

        public int GenerateOTP()
        {
            Random rnd = new Random();
            return (int)rnd.Next(100000, 999999);
        }
        public async Task SendWelcomeEmail(User user)
        {
            var email = user.Email;
            var subject = "Welcome to Project Manager";
            var body = String.Format("Hello {0}, have a fun-filled experience.", user.Name);
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task SendRecoveryEmail(User user)
        {
            var email = user.Email;
            var subject = "Password Reset";
            user.Otp = GenerateOTP();
            await _context.SaveChangesAsync();
            var body = String.Format(
                "Hello {0},\n\nYou have requested for a password reset. Your OTP is {1}.\nIgnore this mail if you didn't request for it.",
                user.Name,
                user.Otp
            );
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public bool ConfirmOTP(User user, int otp)
        {
            return user.Otp == otp;
        }

        public async Task<User> ChangePassword(User user, int otp, string newPassword)
        {
            if(otp == user.Otp)
            {
                user.Hash = BC.HashPassword(newPassword);
                user.Otp = GenerateOTP();
                await _context.SaveChangesAsync();
                return user;
                
            }
            return null;
        }
    }
}