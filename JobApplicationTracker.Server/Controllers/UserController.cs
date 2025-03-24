using JobApplicationTracker.Server.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace JobApplicationTracker.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {

        private ILogger<UserController> _logger;

        private IDbController _dbController;

        public UserController(ILogger<UserController> logger, IDbController dbController)
        {
            _logger = logger; 
            _dbController = dbController;
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="newUser"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task CreateUser([FromBody]NewUserDto newUser)
        {
            await _dbController.AddUserAsync(newUser.Email, newUser.UserName);
        }

        /// <summary>
        /// Gets the specified user info.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetUser(string email)
        {
            var user = await _dbController.GetUserByEmailAsync(email);
            if (user is null)
                return NotFound();

            return Ok(new UserDto(user));
        }

        /// <summary>
        /// Gets a list of all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet("AllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _dbController.GetAllUsersAsync();
            var userDtos = users.Select(u => new UserDto(u)).ToArray();

            return Ok(userDtos);
        }

        /// <summary>
        /// Permanently deletes the specified user.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult> DeleteUser(string email)
        {
            var success = await _dbController.RemoveUserAsync(email);
            if (success)
                return Ok();

            return NotFound();
        }

    }
}
