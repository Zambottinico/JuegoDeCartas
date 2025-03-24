using Juego_Sin_Nombre.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Juego_Sin_Nombre.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginModel login)
        {
            if (login.Username == "admin" && login.Password == "admin123")
            {
                var token = _jwtService.GenerateToken(login.Username, "Admin");
                return Ok(new { token });
            }
            else if (login.Username == "user" && login.Password == "user123")
            {
                var token = _jwtService.GenerateToken(login.Username, "User");
                return Ok(new { token });
            }

            return Unauthorized("Credenciales inválidas");
        }
    }

    public class UserLoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
