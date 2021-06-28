using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.ApplicationCore.Entities
{
    public class Project
    {
        [Key]
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public String Name { get; set; }

        public String Description { get; set; }

        public String[] Technologies { get; set; }

        public String LiveLink { get; set; }

        public String sourceCodeLink { get; set; }
        
    }

}
