using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WechatMall.Api.Controllers
{
    [ApiController]
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        public TestController()
        {

        }

        [Authorize(Roles = "Admin,User")]
        public IActionResult Test()
        {
            string role = User.FindFirst(ClaimTypes.Role)?.Value;
            Guid userID = new Guid(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return Ok();
        }
    }
}
