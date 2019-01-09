namespace Identity.Platform.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserCredentials
    {
        [Required]
        public string Username { get; set; }

        [UIHint("Email")]
        public string Email { get; set; }

        [Required]
        [UIHint("Password")]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Repeat Password")]
        [UIHint("Password")]
        public string RepeatPassword { get; set; }
    }
}