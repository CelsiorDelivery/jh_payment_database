using jh_payment_auth.Models;
using jh_payment_database.Entity;
using jh_payment_database.Model;
using jh_payment_database.Service;
using Microsoft.AspNetCore.Mvc;

namespace jh_payment_database.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/perops/[Controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("adduser")]
        public async Task<ResponseModel> AddUser(User user)
        {
            return await _userService.AddUser(user);
        }

        [HttpDelete("removeuser/{userId}")]
        public async Task<ResponseModel> RemoveUser([FromRoute]long userId)
        {
            return await _userService.DeactivateUser(userId);
        }

        [HttpPut("getuser")]
        public async Task<ResponseModel> GetUser([FromBody] LoginRequest loginRequest)
        {
            return await _userService.GetUser(loginRequest.Email);
        }

        [HttpGet("getall")]
        public async Task<ResponseModel> GetAllUser()
        {
            return await _userService.GetAllUser();
        }

        [HttpGet("getuserbypage/{pageSize}/{pageNumber}/{searchString}/{sortBy}")]
        public async Task<ResponseModel> GetUserByPage([FromRoute] int pageSize, [FromRoute] int pageNumber, [FromRoute] string searchString, [FromRoute] string sortBy)
        {
            return await _userService.GetUserByPageAsync(pageSize, pageNumber, searchString, sortBy);
        }
    }
}
