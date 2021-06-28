using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.ApplicationCore.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Description { get; set; }

        [Required]
        public String[] Technologies { get; set; }

        [Required]
        public String LiveLink { get; set; }

        [Required]
        public String sourceCodeLink { get; set; }
        
    }

}
