using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.DTOs
{
    public class RegisterUserDTO
    {
        
        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
        
        [Required]
        public String ConfirmPassword { get; set; }

        public AccountType AccountType { get; set; }
    }
}
