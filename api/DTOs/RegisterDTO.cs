using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [RegularExpression(@"[^\s]+")]
        public string UserName {get; set;}

        [Required]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Must be at least 8 characters long.")]
        public string Password {get; set;}

        [Required]
        public DateTime BirthDate {get; set;}

        [Required]
        [EmailAddress]
        public string Email {get; set;}
    }
}
