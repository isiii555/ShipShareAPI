using Microsoft.AspNetCore.Mvc;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("signUp")]
        public IActionResult SignUp()
        {
            return Ok();
        }

        [HttpPost("signIn")]
        public IActionResult SignIn()
        {
            return Ok();
        }
    }
}
