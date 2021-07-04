using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.DTOs
{
    public class ConfirmOtpDTO
    {

        [Required]
        public String Email { get; set; }

        [Required]
        public int Otp { get; set; }

    }
}