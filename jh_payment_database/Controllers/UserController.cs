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

        [HttpGet("getuser/{userId}")]
        public async Task<ResponseModel> GetUser([FromRoute] long userId)
        {
            return await _userService.GetUser(userId);
        }
    }
}
