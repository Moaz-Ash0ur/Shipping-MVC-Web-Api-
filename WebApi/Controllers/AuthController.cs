using BL.Contracts;
using BL.DTOs;
using BL.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly TokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IRefreshTokens _refreshTokenService;
        private readonly IRefreshTokensRetrival _refreshTokenRetrival;


        public AuthController(TokenService tokenService, IUserService userService,
        IRefreshTokens refreshTokenService, IRefreshTokensRetrival refreshTokenRetrival)
        {
            _tokenService = tokenService;
            _userService = userService;
            _refreshTokenService = refreshTokenService;
            _refreshTokenRetrival = refreshTokenRetrival;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result.Errors);

            return Ok("User registered successfully");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {

            var userResult = await _userService.LoginAsync(request);
            if (!userResult.Success)
            {
                return Unauthorized("Invalid credentials");
            }

            var claims = await _userService.GetClaims(request.Email);

            var accessToken = _tokenService.GenerateAccessToken(claims.Item1);
            var refreshToken = _tokenService.GenerateRefreshToken();


            RefreshTokenDto newRefreshDto = CreateRefreshTokenObj(claims, refreshToken);

            SetRefreshTokenCookie(refreshToken, newRefreshDto.ExpiresAt);

            return Ok(new { AccessToken = accessToken});
        }


        private RefreshTokenDto CreateRefreshTokenObj((Claim[], UserDto) claims, string refreshToken)
        {
            var newRefreshDto = new RefreshTokenDto
            {
                Token = refreshToken,
                UserId = claims.Item2.Id.ToString(),
                ExpiresAt = DateTime.Now.AddDays(7),
                CurrentState = 0
            };
            _refreshTokenService.Refresh(newRefreshDto);
            return newRefreshDto;
        }


        [HttpPost("refresh-access-token")]
        public async Task<IActionResult> RefreshAccessToken()
        {

            var refreshToken = GetRefreshTokenCookie();

            if (refreshToken.IsNullOrEmpty())
            {
                return Unauthorized("No refresh token found");
            }

            if (_refreshTokenService.IsExpireToken(refreshToken))
            {
                return Unauthorized("Invalid or expired refresh token");
            }

            // Generate a new access token
            var claims = await _userService.GetClaimsById(_refreshTokenRetrival.GetByToken(refreshToken).UserId);

            var newAccessToken = _tokenService.GenerateAccessToken(claims);

            return Ok(new { AccessToken = newAccessToken });
        }




        //Set and Get Token in/From Client-Browser
        private void SetRefreshTokenCookie(string refreshToken,DateTime expires)
        {
            var cookies = new CookieOptions()
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
            };

            Response.Cookies.Append("RefreshToken", refreshToken, cookies);         
        }

        private string GetRefreshTokenCookie()
        {
            return Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) ? refreshToken : string.Empty;
        }



        [HttpGet("confirm-email", Name = "confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _userService.ConfirmEmailAsync(userId, token);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Email confirmed successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse("Email confirmation failed"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var result = await _userService.ForgotPasswordAsync(email);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password reset link sent to your email"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse("Failed to send reset link"));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassDto dto)
        {
            var result = await _userService.ResetPasswordAsync(dto);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password reset successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse("Failed to reset password"));
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassDto dto)
        {
            var result = await _userService.ChangePasswordAsync(dto);

            if (result.Success)
                return Ok(ApiResponse<UserResultDto>.SuccessResponse(result, "Password changed successfully"));

            return BadRequest(ApiResponse<UserResultDto>.FailResponse( "Failed to change password"));
        }



    }




}
