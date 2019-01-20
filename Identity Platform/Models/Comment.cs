namespace Identity.Platform.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Comment
    {
        [Key]
        public string Id { get; set; }

        // User whose wall the comment appears on
        public string OwnerId { get; set; }

        public string AuthorId { get; set; }

        [ForeignKey(nameof(AuthorId))]
        public AppUser Author { get; set; }

        [Required(ErrorMessage = "You cannot post an empty comment.")]
        public string Content { get; set; }

        public DateTime PostedAt { get; set; }
    }
}