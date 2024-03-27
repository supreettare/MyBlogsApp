using MyBlogsApp.Models.Comments;
using System.ComponentModel.DataAnnotations;

namespace MyBlogsApp.Models.Posts
{
    /// <summary>
    /// DTO Class used for Creating a new Post Item, exposed via the API  
    /// </summary>
    public class UpdatePostRequest
    {
        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public List<CreateCommentRequest> Comments { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
