using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.DTOs
{
    public class UpdateUserDTO
    {
        
        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

    }
}
