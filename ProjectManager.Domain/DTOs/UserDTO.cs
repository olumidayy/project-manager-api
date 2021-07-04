using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.Domain.Entities;

namespace ProjectManager.Domain.DTOs
{
    public class UserDTO
    {
        
        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }

        public AccountType AccountType { get; set; }
    }
}
