using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.ApplicationCore.Entities.DTOs
{
    public class UpdateProjectDTO
    {
        [Required]
        public String Name { get; set; }
        
        [Required]
        public String Description { get; set; }

        [Required]
        public String[] Technologies { get; set; }

        [Required]
        public String LiveLink { get; set; }

        [Required]
        public String SourceCodeLink { get; set; }
    }
}
