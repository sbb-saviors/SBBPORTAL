using API.Authorization;
using API.Helpers;
using API.Services;
using CORE.Models;
using CORE.ViewModel.Authenticate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Controllers.Auth
{
    [ApiExplorerSettings(GroupName = "portal-api")]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IUserService _userService;
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }


        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateRequest request)
        {
            try
            {
                var user = _context.ikys_users.SingleOrDefault(x => x.UserName == request.UserName && x.UserStatus == true);
                if (user == null)
                {
                    return BadRequest(new { message = "User not found", statusCode = "400", section = "Auth" });
                }

                var hashedPassword = HashingHelper.ComputeHash(request.UserPassword);
                if (hashedPassword != user.UserPassword)
                {
                    return BadRequest(new { message = "Password error", statusCode = "400", section = "Auth" });
                }

                var response = _userService.Authenticate(request.UserName, hashedPassword, ipAddress());
                return Ok(response);
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred", statusCode = "400", section = "Auth" });
            }
        }


        private string ipAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"]!;
            else
                return HttpContext!.Connection!.RemoteIpAddress!.MapToIPv4().ToString();
        }

    }
}
