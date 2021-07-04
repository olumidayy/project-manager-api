using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.DTOs
{
    public class SendOtpDTO
    {

        [Required]
        public String Email { get; set; }

    }
}