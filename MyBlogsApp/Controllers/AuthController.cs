using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyBlogsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet("token")]
        public IActionResult GetToken()
        {
            //Add custom Auth logic 

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "ExampleUser"), 
                new Claim(JwtRegisteredClaimNames.Email, "user@example.com")
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"])); 
            var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                signingCredentials: signInCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30)
                ); 

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = tokenString });
        }
    }
}
