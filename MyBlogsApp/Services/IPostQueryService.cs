using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Distributed;
using MyBlogsApp.Data;
using MyBlogsApp.Entities;
using Newtonsoft.Json;

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
        private readonly IDistributedCache _distributedCache; 

        public PostQueryService(AppDBContext dbContext, IDistributedCache distributedCache)
        {
            _dbContext = dbContext;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// Get's post by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Post> GetPostById(int id)
        {
            var cacheKey = $"Post_{id}";
            Post post;
            string serializePost;


            try
            {
                //Check if the post details are available in cache 
                var cachedPost = await _distributedCache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(cachedPost))
                {
                    serializePost = cachedPost;
                    post = JsonConvert.DeserializeObject<Post>(serializePost);
                }
                else
                {
                    //Else, fetch latest details from the DB & add in cache 
                    post = await _dbContext.Posts.Include(c => c.Comments).FirstOrDefaultAsync(p => p.Id == id);
                    if (post == null)
                    {
                        return null;
                    }

                    //Ignoring the self referencing loop issue while serializing the object 
                    //to supress error: Self referencing loop detected for property 'Post' with type 'MyBlogsApp.Entities.Post'. Path 'Comments[0]'.
                    var jsonSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

                    serializePost = JsonConvert.SerializeObject(post, jsonSettings);

                    await _distributedCache.SetStringAsync(cacheKey, serializePost, new DistributedCacheEntryOptions
                    {
                        //Setting caching window for 15 mins 
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
                    });
                }
            }
            catch (Exception ex)
            {
                return null;
            }

            //return the post object, from cache or fresh from db 
            return post;
        }
    }
}
