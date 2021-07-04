using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Data.Configuration;
using ProjectManager.Domain.DTOs;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Repositories;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManager.Data.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private JwtConfig _config;
        private IUserRepository _userRepository;
        public AuthenticationRepository(IOptions<JwtConfig> config, IUserRepository userRepository)
        {
            _config = config.Value;
            _userRepository = userRepository;
        }

        public User Authenticate(UserDTO userDTO)
        {
            var user = _userRepository.GetByEmail(userDTO.Email);
            if(user == null) return null;
            if (BC.Verify(userDTO.Password, user.Hash))
            {
                return user;
            }
            return null;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("Id", user.Id.ToString()) }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.UtcNow.AddDays(7)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}