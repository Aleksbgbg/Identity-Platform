namespace Identity.Platform.Models
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class UserCredentials
    {
        public IFormFile Image { get; set; }

        public string UserId { get; set; }

        public string ImageExtension { get; set; }

        [Required]
        public string Username { get; set; }

        [UIHint("Email")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [UIHint("PhoneNumber")]
        public string PhoneNumber { get; set; }

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