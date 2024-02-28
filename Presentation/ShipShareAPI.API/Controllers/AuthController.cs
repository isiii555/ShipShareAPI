using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Auth;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
using ShipShareAPI.Application.Interfaces.Token;
using ShipShareAPI.Domain.Entities;

namespace ShipShareAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;

        public AuthController(IUserManager userManager, ISignInManager signInManager, IRoleManager roleManager)
        {
            _userManager = Guard.Against.Null(userManager);
            _signInManager = Guard.Against.Null(signInManager);
            _roleManager = Guard.Against.Null(roleManager);
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<TokenDto?>> SignUpAsync(SignUpRequest signUpRequest)
        {
            var role = await _roleManager.GetRoleByName("User");
            var user = new User()
            {
                Email = signUpRequest.Email,
                Username = signUpRequest.UserName,
                Roles = new List<Role> { role! }
            };
            var result = await _userManager.CreateAsync(user,signUpRequest.Password);

            return result ? Ok(await _signInManager.SignInAsync(user.Email,signUpRequest.Password)) : BadRequest("Sign up failed");
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<TokenDto>> SignInAsync(SignInRequest signInRequest)
        {
            var user = _userManager.FindByEmailAsync(signInRequest.Email);
            if (user is not null)
            {
                return BadRequest("User not found!");
            }
            else 
                return Ok(await _signInManager.SignInAsync(signInRequest.Email, signInRequest.Password));
        }
    }
}
