﻿using Ardalis.GuardClauses;
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
            return BadRequest("User with this email already exist!");
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<TokenDto>> SignInAsync(SignInRequest signInRequest)
        {
            var user = await _userManager.FindByEmailAsync(signInRequest.Email);
            if (user is not null)
            {
                var token = await _signInManager.SignInAsync(user, signInRequest.Password);
                return Ok(token);
            }
            return BadRequest("Email is wrong!");
        }

        [HttpPost("refreshTokenSignIn")]
        public async Task<ActionResult<TokenDto>> RefreshTokenSignAsync([FromForm]string refreshToken)
        {
            return Ok(await _signInManager.RefreshTokenSignInAsync(refreshToken));
        }
    }
}