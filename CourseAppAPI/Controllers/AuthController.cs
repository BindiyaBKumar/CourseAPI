using CourseAppAPI.DTO;
using CourseAppAPI.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CourseAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JWTTokenService _jwtservice;
        public AuthController(JWTTokenService jwtservice)
        {
            _jwtservice = jwtservice;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO loginrequest)        
        {
            if (string.IsNullOrEmpty(loginrequest.username) || string.IsNullOrEmpty(loginrequest.password)) 
            {
                return BadRequest("Username and password cannot be empty");
            }
            if(loginrequest.username.Equals("user") && loginrequest.password.Equals("password"))
            {
                var token = _jwtservice.Genratetoken(loginrequest.username);
                return Ok(new {Token = token});
            }
            return BadRequest("Invalid username or password");
        }

    }
}
