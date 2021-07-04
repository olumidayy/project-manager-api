using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProjectManager.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public String Email { get; set; }

        public AccountType AccountType { get; set; }

        [JsonIgnore]
        public int Otp { get; set; }
        
        [JsonIgnore]
        public String Hash { get; set; }
    }
}
