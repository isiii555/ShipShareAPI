using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using ShipShareAPI.Application.Dto.Auth;
using ShipShareAPI.Application.Dto.Token;
using ShipShareAPI.Application.Interfaces.Auth;
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
            var oldUser = await _userManager.FindByEmailAsync(signUpRequest.Email);
            if (oldUser is null)
            {
                var user = new User()
                {
                    Email = signUpRequest.Email,
                    Username = signUpRequest.UserName,
                };
                var token = await _userManager.CreateAsync(user, signUpRequest.Password);
                await _userManager.UpdateRefreshToken(user, token.RefreshToken, token.Expiration);
                return Ok(token);
            }
            return BadRequest(new { status = "User with this email already exist!" });
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<TokenDto>> SignInAsync(SignInRequest signInRequest)
        {
            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user is not null)
            {
                var token = await _signInManager.SignInAsync(user, signInRequest.Password);

                return token is not null ? Ok(token) : BadRequest(new {status = "Password is wrong"});
            }
            return BadRequest(new {status = "Email is wrong"});
        }

        [HttpPost("refreshTokenSignIn")]
        public async Task<ActionResult<TokenDto>> RefreshTokenSignAsync()
        {
            return Ok(await _signInManager.RefreshTokenSignInAsync());
        }

        [HttpGet("getUserDetailsWithId/{userId}")]
        public async Task<ActionResult<User>> GetUsernameWithId(Guid userId)
        {
            var user = await _userManager.GetUserWithId(userId);
            return user is not null ? Ok(user) : BadRequest("User not found");
        }

        [HttpGet("confirmEmail/{userId}/{token}")]
        public async Task<ActionResult<bool>> ConfirmEmail(Guid userId, string token)
        {
            var result = await _userManager.ConfirmEmail(userId, token);
            return result ? Ok(result) : BadRequest(result);
        }

        [HttpPost("forgotPassword")]
        public async Task<ActionResult<bool>> ForgotPassword([FromBody] string email)
        {
            var result = await _userManager.SendForgotPasswordEmail(email);
            return result ? Ok(result) : BadRequest("User not found");
        }

        [HttpPost("resetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordRequest resetPasswordRequest)
        {
            var result = await _userManager.ResetPassword(resetPasswordRequest);
            return result.IsChanged ? Ok(new { status = result.Message }) : BadRequest(new{ status = result.Message });
        }

        [HttpGet("getMyDetails")]
        public async Task<ActionResult<User>> GetMyDetails()
        {
            var user =  await _userManager.GetMyDetails();
            return user is not null ? Ok(user) : BadRequest();
        }
    }
}
    