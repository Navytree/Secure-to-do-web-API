using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoUserWebAPI.Data;
using ToDoUserWebAPI.DTOs;
using ToDoUserWebAPI.Extensions;
using ToDoUserWebAPI.Models;

namespace ToDoUserWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ToDoContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public AccountController(ToDoContext context, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Login == dto.Login))
            { return BadRequest("Login is already taken."); }

            var user = new User
            {
                Login = dto.Login,
                PasswordHash = _passwordHasher.Hash(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string token = _tokenService.CreateJwtToken(user);

            return Ok(new { Token = token, UserId = user.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);

            if (user == null || !_passwordHasher.Verify(dto.Password, user.PasswordHash))
            { return Unauthorized("Invalid login or password."); }

            string token = _tokenService.CreateJwtToken(user);

            return Ok(new { Message = "Logged in successfully", Token = token, UserId = user.Id });
        }



    }
}
