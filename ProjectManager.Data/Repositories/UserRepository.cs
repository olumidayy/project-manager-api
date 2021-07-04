using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using ProjectManager.Domain.Services;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManager.Data.Repositories
{
    public class UserRepository : IUserRepository

    {
        private ApiDbContext _context;
        private IMailerService _mailerService;

        public UserRepository(ApiDbContext context, IMailerService mailerService)
        {
            _context = context;
            _mailerService = mailerService;
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
            await _mailerService.SendEmailAsync(user.Email, subject, body);
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
            await _mailerService.SendEmailAsync(user.Email, subject, body);
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