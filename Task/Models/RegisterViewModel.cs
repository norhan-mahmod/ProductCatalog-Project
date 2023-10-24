using System.ComponentModel.DataAnnotations;

namespace ProductTask.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Address!")]
        public string Email { get; set; }
        [Required]
        [StringLength(15 , MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [Compare("Password" , ErrorMessage ="Password MisMatch!")]
        public string ConfirmPassword { get; set; }
    }
}
