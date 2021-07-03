using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.ApplicationCore.Enums;

namespace ProjectManager.ApplicationCore.Entities.DTOs
{
    public class ChangePasswordDTO
    {

        [Required]
        public String Email { get; set; }

        [Required]
        public int Otp { get; set; }
        
        [Required]
        public String NewPassword { get; set; }

        public AccountType AccountType { get; set; }
    }
}