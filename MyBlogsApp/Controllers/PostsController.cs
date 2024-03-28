using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlogsApp.Entities;
using MyBlogsApp.Models.Posts;
using MyBlogsApp.Services;

namespace MyBlogsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IPostCommandService _postCommandService;
        private readonly IPostQueryService _postQueryService;

        public PostsController(IPostCommandService postCommandService, IPostQueryService postQueryService)
        {
            _postCommandService = postCommandService;
            _postQueryService = postQueryService;
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreatePost(CreatePostRequest request)
        {
            if (request == null) 
            { 
                return BadRequest(); 
            }

            var result = await _postCommandService.CreatePost(request);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            var post = await _postQueryService.GetPostById(id);
            return Ok(post);
        }
    }
}
