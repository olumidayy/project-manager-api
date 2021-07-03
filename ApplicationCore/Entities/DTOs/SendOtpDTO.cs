using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.ApplicationCore.Enums;

namespace ProjectManager.ApplicationCore.Entities.DTOs
{
    public class SendOtpDTO
    {

        [Required]
        public String Email { get; set; }

    }
}