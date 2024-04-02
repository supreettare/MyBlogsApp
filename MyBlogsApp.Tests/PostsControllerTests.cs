using Microsoft.AspNetCore.Mvc;
using Moq;
using MyBlogsApp.Controllers;
using MyBlogsApp.Entities;
using MyBlogsApp.Models.Posts;
using MyBlogsApp.Services;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogsApp.Tests
{
    public class PostsControllerTests
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreatePost_ReturnPostId()
        {
            //Arrange 
            var mockService = new Mock<IPostCommandService>();
            mockService.Setup(s => s.CreatePost(It.IsAny<CreatePostRequest>())).ReturnsAsync(1);

            var controller = new PostsController(mockService.Object, null); // we don't need to use the IQueryPostService here 

            //Act 
            var result = await controller.CreatePost(new CreatePostRequest());

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(1, okResult.Value); 
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPostById_ReturnPost()
        {
            //Arrange 
            var mockService = new Mock<IPostQueryService>();
            var testPost = new Post { Id = 1, Title = "Test Title", Content = "Test Content" };           
            mockService.Setup(s => s.GetPostById(1)).ReturnsAsync(testPost);           

            var controller = new PostsController(null, mockService.Object); // we don't need to use the IPostCommandService here 

            //Act
            var result = await controller.GetPostById(1); 

            //Assert
            Assert.NotNull(result);

            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult); 

            var returnedPost = okResult.Value as Post;
            Assert.NotNull(returnedPost);

            Assert.Equal(1, returnedPost.Id); 
            Assert.Equal("Test Title", returnedPost.Title);
            Assert.Equal("Test Content", returnedPost.Content);

        }
    }
}
