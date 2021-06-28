using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.ApplicationCore.Configuration;
using ProjectManager.ApplicationCore.Entities;
using ProjectManager.ApplicationCore.Entities.DTOs;
using BC = BCrypt.Net.BCrypt;

namespace ProjectManager.ApplicationCore.Services
{
    public interface IAuthenticationService
    {
        User Authenticate(UserDTO userDTO);
        string GenerateJwtToken(User user);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private JwtConfig _config;
        private IUserService _userService;
        public AuthenticationService(IOptions<JwtConfig> config, IUserService userService)
        {
            _config = config.Value;
            _userService = userService;
        }

        public User Authenticate(UserDTO userDTO)
        {
            var user = _userService.GetByEmail(userDTO.Email);
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