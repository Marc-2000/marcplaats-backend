using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Marcplaats_Backend.BLL.DTOs;
using Marcplaats_Backend.BLL.Models.Entities;
using Marcplaats_Backend.BLL.RepositoryInterfaces;
using Marcplaats_Backend.DAL.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace Marcplaats_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]UserRegisterDTO user)
        {
            ServiceResponse<Guid> response = await _accountRepository.Register(
                new User
                { Username = user.Username, Email = user.Email, PhoneNumber = user.PhoneNumber }, 
                user.Password
                );
            return Ok(response);
        }

        [HttpPost("RegisterAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdminRegisterDTO user)
        {
            ServiceResponse<Guid> response = await _accountRepository.RegisterAdmin(
                new User
                { Username = user.Username },
                user.Password
                );
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserLoginDTO user)
        {
            ServiceResponse<User> response = await _accountRepository.Login(user.Username, user.Password);
            return Ok(response);
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDTO user)
        {
            ServiceResponse<User> response = new ServiceResponse<User>();

            Payload payload;
            try
            {
                payload = await ValidateAsync(user.IDToken, new ValidationSettings
                {
                    Audience = new[] { "992372443362-d9fuim7eg9o2lrnk4ir0fpcf7iv8udvn.apps.googleusercontent.com" } //TODO nog in appsettings toevoegen.
                });
                if (payload != null)
                {
                    response = await _accountRepository.GoogleLogin(payload);
                    if (!response.Success) return BadRequest(response);
                    return Ok(response);
                }
                response.Message = "Google Id_Token invalid";
                response.Success = false;
                response.Data = null;
                return Ok(response);
            }
            catch (Exception)
            {
                response.Message = "Google Id_Token invalid";
                response.Success = false;
                response.Data = null;
                return Ok(response);
            }
        }

        [Authorize]
        [HttpGet("byID/{id}")]
        public async Task<IActionResult> GetUserByID(string id)
        {
            try
            {
                User user = await _accountRepository.Get(Guid.Parse(id));
                return Ok(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                List<User> users = await _accountRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("DeleteUser/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                ServiceResponse<User> response = await _accountRepository.DeleteUser(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
