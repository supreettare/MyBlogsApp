namespace MyBlogsApp.Entities
{
    /// <summary>
    /// The Comment entity, not exposed via the API 
    /// </summary>
    public class Comment : Base
    {
        public string Author { get; set; }

        public string Content { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }
    }
}
