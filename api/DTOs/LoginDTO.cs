using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class LoginDTO
    {
        [Required]
        [RegularExpression(@"[^\s]+")]
        public string UserName {get; set;}
        
        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Must be at least 8 characters long.")]
        public string Password {get; set;}
    }
}