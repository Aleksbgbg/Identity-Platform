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

        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }
    }
}