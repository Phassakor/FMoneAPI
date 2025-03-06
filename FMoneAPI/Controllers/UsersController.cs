using FMoneAPI.Services.UserService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FMoneAPI.DTOs;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Cms;

namespace FMoneAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDTO>> GetTotalUser()
        {
            var user = await _userService.GetTotalUsers();
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new { status = 200, data = user});
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            var result = await _userService.CreateUserAsync(request);

            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { status = 200 ,message = result.message });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequestDTO request, string password = null)
        {
            if (await _userService.UpdateUser(request, password))
                return Ok(new
                {
                    status = 200,
                    message = "Update User successfully"
                });

            return BadRequest(new { message = "Some think wrong naja" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.DeleteUser(id))
                return Ok(new
                {
                    status = 200,
                    message = "Delete User successfully"
                });

            return BadRequest(new { message = "Some think wrong naja" });
        }
        [HttpPatch("updatePermision")]
        public async Task<IActionResult> UpdatePermision([FromBody] UpdateUserPermissionDTO request)
        {
            var result = await _userService.UpdateUserPermissionsAsync(request);

            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { status = 200, message = result.message });
        }

        [HttpGet("{userId}/MenuPermissions")]
        public async Task<IActionResult> GetUserPermissionsMenu(int userId)
        {
            var permissions = await _userService.GetUserPermissionsMenuByUserId(userId);

            if (permissions == null || !permissions.Any())
            {
                return NotFound(new { message = "No permissions found for this user." });
            }
            var newArray = permissions.Select(ump => new
            {
                Id = ump.Id,
                UserId = ump.UserId,
                MenuId = ump.MenuId,
                MenuName = ump.Menu.Name,
                ParentId = ump.Menu.ParentId,
                CanAccess = ump.CanAccess
            }).ToArray();

            return Ok(new { status = 200, data = newArray });
        }
        [HttpGet("GetMenusAll")]
        public async Task<IActionResult> GetTotalMenus()
        {
            var menus = await _userService.GetTotalMenus();
            if (menus == null)
            {
                return NotFound();
            }
            var newArray = menus.Select(ump => new
            {
                MenuId = ump.Id,
                Name = ump.Name,
                parentId = ump.ParentId
            }).ToArray();

            return Ok(new { status = 200, data = newArray });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            var Data = await _userService.Login(username, password);
            if (Data == null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(new
            {
                status = 200,
                Data
            });
        }
    }
}
