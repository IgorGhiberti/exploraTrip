using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Users;
using Application.Users.DTOs;
using Domain.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Helpers;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            List<ShowUserDTO> users = await _userServices.GetAll();
            if (users == null)
            {
                return NotFound("Nenhum usuário registrado");
            }
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
        {
            string hashPassword = Cryptography.CreateHash(userDto.Password, userDto.EmailVal);
            var user = userDto with { Password = hashPassword };
            await _userServices.AddUser(user, cancellationToken);
            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> LoginUser(LoginUserDTO userDto, CancellationToken cancellationToken)
        {
            string storedHash = await _userServices.GetLoginInfo(userDto.Id);
            bool isCorrectPassword = Cryptography.ValidateHash(userDto.Password, storedHash, userDto.Email);
            return isCorrectPassword ? Ok("Autenticado.") : BadRequest("Senha errada");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
        {
            await _userServices.UpdateUser(id, userDto, cancellationToken);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userServices.GetUserById(id);
            if (user == null)
            {
                return NotFound("Esse usuário não existe.");
            }
            return Ok(user);
        }

        [HttpGet("isActive/{id}")]
        public async Task<IActionResult> IsUserActive(Guid id)
        {
            bool isUserActive = await _userServices.IsUserActive(id);
            return isUserActive ? Ok("Usuário está ativo.") : Ok("Usuário não ativo.");
        }

        [HttpPut("activeUser/{id}")]
        public async Task<IActionResult> ActiveUser(Guid id)
        {
            await _userServices.ActiveUser(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            await _userServices.DisableUser(id);
            return Ok();
        }
    }
}