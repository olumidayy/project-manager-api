using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Domain.DTOs
{
    public class ProjectDTO
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public int OwnerId { get; set; }
        
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
