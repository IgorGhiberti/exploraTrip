using Application.Enum;
using Application.Interfaces;
using Application.Users;
using Application.Users.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Get all users registered in the system.
        /// </summary>
        /// <returns>List of users</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userServices.GetAll();
            return users.ToOkResult();
        }

        /// <summary>
        /// Register a new user in the system.
        /// </summary>
        /// <param name="userDto">User body</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>User data</returns>
        /// <response code="201">Created</response>
        /// <response code="400">BadRequest</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUser(CreateUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AddUser(userDto, cancellationToken);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToCreateResult($"api/user/{result.Data!.Id}");
        }

        /// <summary>
        /// Authenticate a user in the system.
        /// </summary>
        /// <param name="userDto">User body</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Indicates whether the login was successful.</returns>
        /// <response code="200">Ok</response>
        /// <response code="401">Unauthorized</response>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginUser(LoginUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.AuthenticateUser(userDto);
            if (!result.IsSuccess)
                return result.ToUnauthorizedResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Update user data.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDto">User body</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>New user data</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.UpdateUser(id, userDto, cancellationToken);
            return result.ToSingleResult();
        }

        /// <summary>
        /// Get a specific user by id.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User data</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _userServices.GetUserById(id);
            return result.ToSingleResult();
        }

        /// <summary>
        /// Check whether the user is active.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>If is active or not</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpGet("isActive/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> IsUserActive(Guid id)
        {
            var result = await _userServices.IsUserActive(id);
            return result.ToSingleResult();
        }

        /// <summary>
        /// Activate a user account.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User data</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpPut("activeUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ActiveUser(Guid id)
        {
            var result = await _userServices.ActiveUser(id);
            return result.ToSingleResult();
        }

        /// <summary>
        /// Disable user.
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User data</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DisableUser(Guid id)
        {
            var result = await _userServices.DisableUser(id);
            return result.ToSingleResult();
        }

        /// <summary>
        /// Update user password.
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="userDto">User body</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>A message indicating whether the update was successful.</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response> 
        [HttpPut("updatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(Guid id, UpdatePasswordDTO userDto, CancellationToken cancellationToken)
        {
            var result = await _userServices.UpdatePassword(id, userDto, cancellationToken);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Confirm the code sent by email.
        /// </summary>
        /// <param name="userDto">User body</param>
        /// <param name="operationNumber">Operation number</param>
        /// <returns>A true or false value</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response> 
        [HttpPost("confirmCode/{operationNumber}")]
        public async Task<IActionResult> ConfirmCode(ConfirmUserCodeDTO userDto, int operationNumber)
        {
            var result = await _userServices.ConfirmEmailCode(userDto.Email, userDto.Code, (OperationEnum)operationNumber);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Update the user's password when they forget the current one.
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <returns>Indicates whether the operation was successful.</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response> 
        [HttpPost("forgotPassword/{userEmail}")]
        public async Task<IActionResult> ForgotPassword(string userEmail)
        {
            var result = await _userServices.ForgetPassword(userEmail);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToOkResult();
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        /// <param name="userDto">User body</param>
        /// <returns>If the operation was succeful</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">BadRequest</response>
        [HttpPut("resetPassword")]
        public async Task<IActionResult> ResetPassword(UpdatePasswordDTO userDto)
        {
            var result = await _userServices.ResetPassword(userDto);
            if (!result.IsSuccess)
                return result.ToBadRequestResult();
            return result.ToOkResult();
        }
    }
}