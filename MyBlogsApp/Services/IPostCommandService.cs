using MyBlogsApp.Data;
using MyBlogsApp.Entities;
using MyBlogsApp.Models.Comments;
using MyBlogsApp.Models.Posts;

namespace MyBlogsApp.Services
{
    public interface IPostCommandService
    {
        Task<int> CreatePost(CreatePostRequest request);
    }

    /// <summary>
    /// Post Command Service for creating new posts
    /// </summary>
    public class PostCommandService : IPostCommandService
    {
        private readonly AppDBContext _dbContext;
        public PostCommandService(AppDBContext dbContext)
        {
                _dbContext = dbContext;
        }
        public async Task<int> CreatePost(CreatePostRequest request)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                Comments = request.Comments.Select(c => new Comment
                {
                    Author = c.Author,
                    Content = c.Content
                }).ToList()
            };

            _dbContext.Posts.Add(post);
            await _dbContext.SaveChangesAsync();   
            return post.Id;
        }
    }
}
