using System;
using System.ComponentModel.DataAnnotations;
using ProjectManager.ApplicationCore.Enums;

namespace ProjectManager.ApplicationCore.Entities.DTOs
{
    public class UpdateUserDTO
    {
        
        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

    }
}
