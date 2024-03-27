using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MyBlogsApp.Data;
using MyBlogsApp.Entities;

namespace MyBlogsApp.Services
{
    public interface IPostQueryService
    {
        Task<Post> GetPostById(int id);
    }

    /// <summary>
    /// Post query service for querying posts 
    /// </summary>
    public class PostQueryService : IPostQueryService
    {
        private readonly AppDBContext _dbContext;
        public PostQueryService(AppDBContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<Post> GetPostById(int id)
        {
            var post = await _dbContext.Posts.Include(c=>c.Comments).FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
            {
                return null; 
            }

            return post;
        }
    }
}
