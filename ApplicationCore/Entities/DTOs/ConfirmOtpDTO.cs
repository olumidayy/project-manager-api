using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.ApplicationCore.Enums;

namespace ProjectManager.ApplicationCore.Entities.DTOs
{
    public class ConfirmOtpDTO
    {

        [Required]
        public String Email { get; set; }

        [Required]
        public int Otp { get; set; }

    }
}