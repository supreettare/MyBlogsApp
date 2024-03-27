namespace MyBlogsApp.Models.Comments
{
    /// <summary>
    /// DTO Class for creating a new comment, exposed via the API 
    /// </summary>
    public class CreateCommentRequest
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
