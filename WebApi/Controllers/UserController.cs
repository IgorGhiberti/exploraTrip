using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
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
        private readonly string baseUri = "http://localhost:5052/api/user";
        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServices.GetAll();
            if (!users.IsSuccess)
                return users.ToNotFoundResult();
            return users.ToOkResult();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AddUser(userDto, cancellationToken);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToCreateResult($"{baseUri}/{result.Data!.Id}");
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> LoginUser(LoginUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthenticateUser(userDto);
            if (!result.IsSuccess)
                return result.ToUnauthorizedResult();
            return result.ToOkResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.UpdateUser(id, userDto, cancellationToken);
            return result.ToSingleResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userServices.GetUserById(id);
            return result.ToSingleResult();
        }

        [HttpGet("isActive/{id}")]
        public async Task<IActionResult> IsUserActive(Guid id)
        {
            var result = await _userServices.IsUserActive(id);
            return result.ToSingleResult();
        }

        [HttpPut("activeUser/{id}")]
        public async Task<IActionResult> ActiveUser(Guid id)
        {
            var result = await _userServices.ActiveUser(id);
            return result.ToSingleResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            var result = await _userServices.DisableUser(id);
            return result.ToSingleResult();
        }
    }
}