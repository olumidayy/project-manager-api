using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.DTOs
{
    public class SignInUserDTO
    {        
        [Required]
        public String Email { get; set; }

        [Required]
        public String Password { get; set; }
    }
}
