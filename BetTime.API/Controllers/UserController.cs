using Microsoft.AspNetCore.Mvc;
using BetTime.Business;
using BetTime.Models;
using Microsoft.AspNetCore.Authorization;

namespace BetTime.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(ILogger<UserController> logger, IUserService userService, IAuthService authService)
        {
            _logger = logger;
            _userService = userService;
            _authService = authService;
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting all users: {ex.Message}");
                return BadRequest($"Error getting all users: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("byEmail")]
        public IActionResult GetUserByEmail(string email)
        {
            try
            {
                var user = _userService.GetUserByEmail(email);
                return Ok(user);
            }
            catch (KeyNotFoundException knf)
            {
                _logger.LogWarning($"User not found with email {email}: {knf.Message}");
                return NotFound($"User not found with email {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user by email: {ex.Message}");
                return BadRequest($"Error getting user by email: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        [HttpGet("{userId}")]
        public IActionResult GetUserById(int userId)
        {
            if (!_authService.HasAccessToResource(userId, HttpContext.User))
                return Forbid();

            try
            {
                var user = _userService.GetUserById(userId);
                return Ok(user);
            }
            catch (KeyNotFoundException knf)
            {
                _logger.LogWarning($"User not found with id {userId}: {knf.Message}");
                return NotFound($"User not found with id {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting user by id: {ex.Message}");
                return BadRequest($"Error getting user by id: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        [HttpPut("{userId}")]
        public IActionResult UpdateUser(int userId, [FromBody] UserUpdateDTO updateDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authService.HasAccessToResource(userId, HttpContext.User))
                return Forbid();

            try
            {
                _userService.UpdateUser(userId, updateDTO);
                return Ok($"User with id {userId} updated successfully");
            }
            catch (KeyNotFoundException knf)
            {
                _logger.LogWarning($"User not found with id {userId}: {knf.Message}");
                return NotFound($"User not found with id {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user: {ex.Message}");
                return BadRequest($"Error updating user: {ex.Message}");
            }
        }

        [Authorize(Roles = Roles.Admin + "," + Roles.User)]
        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(int userId)
        {
            if (!_authService.HasAccessToResource(userId, HttpContext.User))
                return Forbid();

            try
            {
                _userService.DeleteUser(userId);
                return Ok($"User with id {userId} deleted successfully");
            }
            catch (KeyNotFoundException knf)
            {
                _logger.LogWarning($"User not found with id {userId}: {knf.Message}");
                return NotFound($"User not found with id {userId}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user: {ex.Message}");
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }

        
}
}