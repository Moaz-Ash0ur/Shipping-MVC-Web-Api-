using BL.DTOs;
using DAL.UserModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contracts
{
    public interface IUserService
    {
        Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
        Task<UserResultDto> LoginAsync(LoginDto loginDto);
        Task LogoutAsync();

        Task<UserResultDto> ConfirmEmailAsync(string userId, string token);
        Task<UserResultDto> ForgotPasswordAsync(string email);
        Task<UserResultDto> ResetPasswordAsync(ResetPassDto dto);
        Task<UserResultDto> ChangePasswordAsync(ChangePassDto dto);

        Task<UserResultDto> UpdateUserAsync(EditUserDto user);

        Task<UserDto> GetUserByIdAsync(string userId);
        Task<UserDto> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Guid GetLoggedInUser();
        Task<(Claim[], UserDto)> GetClaims(string email);
        Task<Claim[]> GetClaimsById(string userId);
    }

}
