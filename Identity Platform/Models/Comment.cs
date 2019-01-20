namespace Identity.Platform.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        // User whose wall the comment appears on
        public string OwnerId { get; set; }

        public string AuthorId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}