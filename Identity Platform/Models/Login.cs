namespace Identity.Platform.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Login
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [UIHint("Password")]
        public string Password { get; set; }
    }
}