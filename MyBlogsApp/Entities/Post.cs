using System.Text.Json;

namespace MyBlogsApp.Entities
{
    /// <summary>
    /// The Post entity. This entity is not exposed via the APIs 
    /// </summary>
    public class Post : Base
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<Comment> Comments { get; set; }
    }

}
