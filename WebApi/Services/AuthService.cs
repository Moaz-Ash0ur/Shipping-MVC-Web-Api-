using BL.Contracts;
using BL.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.Services
{

    //public interface IAuthService
    //{
    //    Task<UserResultDto> RegisterAsync(RegisterDto request);
    //    Task<AuthResult> LoginAsync(LoginDto request);
    //    Task<AuthResult> RefreshAccessTokenAsync(string refreshToken);
    //    string GenerateAccessToken(IEnumerable<Claim> claims);
    //    string GenerateRefreshToken();
    //}

    //public class AuthResult
    //{
    //    public bool Success { get; set; }
    //    public string AccessToken { get; set; }
    //    public string RefreshToken { get; set; }
    //    public DateTime? ExpiresAt { get; set; }
    //    public IEnumerable<string> Errors { get; set; }

    //    public AuthResult()
    //    {
    //        Errors = new List<string>();
    //    }
    //}

    //public class AuthService : IAuthService
    //{
    //    private readonly IUserService _userService;
    //    private readonly IRefreshTokens _refreshTokenService;
    //    private readonly IConfiguration _config;
    //    private readonly IRefreshTokensRetrival _refreshTokenRetrival;


    //    public AuthService(TokenService tokenService, IUserService userService,
    //        IRefreshTokens refreshTokenService, IConfiguration configuration, IRefreshTokensRetrival refreshTokenRetrival)
    //    {
    //        _userService = userService;
    //        _refreshTokenService = refreshTokenService;
    //        _config = configuration;
    //        _refreshTokenRetrival = refreshTokenRetrival;
    //    }

    //    public async Task<UserResultDto> RegisterAsync(RegisterDto request)
    //    {
    //        return await _userService.RegisterAsync(request);
    //    }

    //    public async Task<AuthResult> LoginAsync(LoginDto request)
    //    {
    //        var userResult = await _userService.LoginAsync(request);
    //        if (!userResult.Success)
    //        {
    //            return new AuthResult { Success = false, Errors = new[] { "Invalid credentials" } };
    //        }

    //        var claims = await _userService.GetClaims(request.Email);
    //        var accessToken = GenerateAccessToken(claims.Item1);          
    //        var refreshToken = GenerateRefreshToken();

    //        var  expiresAt = DateTime.UtcNow.AddDays(7);

    //        var newRefreshDto = new RefreshTokenDto
    //        {
    //            Token = refreshToken,
    //            UserId = claims.Item2.Id.ToString(),
    //            ExpiresAt = expiresAt,
    //            CurrentState = 0
    //        };

    //        _refreshTokenService.Refresh(newRefreshDto);
            
    //        return new AuthResult
    //        {
    //            Success = true,
    //            AccessToken = accessToken,
    //            RefreshToken = refreshToken,
    //           ExpiresAt = expiresAt
    //        };

    //    }
  
    //    public async Task<AuthResult> RefreshAccessTokenAsync(string refreshToken)
    //    {
    //        if (string.IsNullOrEmpty(refreshToken))
    //        {
    //            return new AuthResult { Success = false, Errors = new[] { "No refresh token found" } };
    //        }

    //        if (_refreshTokenService.IsExpireToken(refreshToken))
    //        {
    //            return new AuthResult { Success = false, Errors = new[] { "Invalid or expired refresh token" } };
    //        }

    //        var refreshData = _refreshTokenRetrival.GetByToken(refreshToken);
    //        var claims = await _userService.GetClaimsById(refreshData.UserId);
    //        var newAccessToken = GenerateAccessToken(claims);

    //        return new AuthResult
    //        {
    //            Success = true,
    //            AccessToken = newAccessToken
    //        };
    //    }



    //    public string GenerateAccessToken(IEnumerable<Claim> claims)
    //    {
    //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:SecretKey"]!));
    //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    //        var token = new JwtSecurityToken(
    //            issuer: _config["JwtSettings:Issuer"],
    //            audience: _config["JwtSettings:Audience"],
    //            claims: claims,
    //            expires: DateTime.Now.AddMinutes(15),
    //            signingCredentials: creds
    //        );

    //        return new JwtSecurityTokenHandler().WriteToken(token);
    //    }

    //    public string GenerateRefreshToken()
    //    {
    //        var randomNumber = new byte[32];
    //        using (var rng = RandomNumberGenerator.Create())
    //        {
    //            rng.GetBytes(randomNumber);
    //        }
    //        return Convert.ToBase64String(randomNumber);
    //    }










    //}


}
