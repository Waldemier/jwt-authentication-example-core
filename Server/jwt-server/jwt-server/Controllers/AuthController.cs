using System;
using jwt_server.Dtos;
using jwt_server.Helpers;
using jwt_server.Models;
using jwt_server.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace jwt_server.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository repo, JwtService jwtService)
        {
            this._repo = repo;
            this._jwtService = jwtService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDto model)
        {
            var user = new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            return Created("Success", this._repo.Create(user));
        }

        [HttpPost("login")]
        public IActionResult Login(LoginDto model)
        {
            var user = this._repo.GetByEmail(model.Email);
            
            if (user == null) return BadRequest(new { message="Invalid Credentials" });
            
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return BadRequest(new {message = "Invalid Credentials"});
            }

            var jwtToken = this._jwtService.Generate(user.Id);
            
            // Sends token in the cookie to a client
            HttpContext.Response.Cookies.Append("jwt", jwtToken, new CookieOptions()
            {
                HttpOnly = true, // Цей параметр дає змогу клінтській частині тільки приймати та відправляти наявкий cookie, нічого більше.
                                 // Також стає неможливим отримати токен через JS.
                                 // Всі маніпуляції з токеном проводяться тільки на стороні серверу.
                                 
                SameSite = SameSiteMode.Strict, // Параметр для захисту від CSRF-атак. Забороняє передачу Cookie-файлів, якщо перехід до поточного API був не з встановленого в Cookie домену.
                
                Secure = true // Для передачі тільки через http. Посилює захист від снифінгу. 
            });
            
            return Ok(new
            {
                message = "success"
            });
        }

        [HttpGet("user")]
        public IActionResult User()
        {
            try
            {
                var Headers = HttpContext.Request.Headers;
                var jwt = HttpContext.Request.Cookies["jwt"]; // get from a client
                var verifyToken = this._jwtService.Verify(jwt);

                var userId = int.Parse(verifyToken.Issuer);
                var user = this._repo.GetById(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt"); // send to a client 

            return Ok(new
            {
                message = "success"
            });
        }
    }
}