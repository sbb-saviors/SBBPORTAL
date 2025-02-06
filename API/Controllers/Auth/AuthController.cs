using API.Authorization;
using API.Helpers;
using API.Services;
using CORE.Models;
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
        public IActionResult Authenticate([BindRequired] string UserName, [BindRequired] string UserPassword)
        {

            try
            {
                var asdasd = _context.ikys_users.Where(x => x.UserName == UserName).FirstOrDefault();
                var user = _context.ikys_users.SingleOrDefault(x => x.UserName == UserName && x.UserStatus == true);
                var hashedPassword = HashingHelper.ComputeHash(UserPassword);
                if (hashedPassword != user.UserPassword)
                {
                    return BadRequest(new { message = "Password erorr", statusCode = "400", section = "Auth" });
                }
                else
                {
                    var response = _userService.Authenticate(UserName, hashedPassword, ipAddress());
                    return Ok(response);
                }
            }
            catch (Exception)
            {

                return BadRequest(new {  message = "", statusCode = "400", section = "Auth" });

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
