 using BL.Contracts;
using BL.DTOs;
using DAL.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BL.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokensRetrival _refreshTokensRetrival;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor, IRefreshTokensRetrival refreshTokensRetrival)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokensRetrival = refreshTokensRetrival;
        }



        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                return new UserResultDto { Success = false, Errors = new[] { "Passwords do not match." } };
            }

            var user = new ApplicationUser 
            { 
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.Email, 
                Email = registerDto.Email,
                PhoneNumber = registerDto.Phone,    
                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
     
            var role = string.IsNullOrEmpty(registerDto.Role) ? "User" : registerDto.Role;

            var assignRole = await _userManager.AddToRoleAsync(user, role)!;

            if (!assignRole.Succeeded)
            {
                return new UserResultDto
                {
                    Success = assignRole.Succeeded,
                    Errors = assignRole.Errors?.Select(e => e.Description)!
                };
            }

            return new UserResultDto
            {
                Success = result.Succeeded,
                Errors = result.Errors?.Select(e => e.Description)!
            };

        }

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

            if (!result.Succeeded)
            {
                return new UserResultDto
                {
                    Success = false,
                    Errors = new[] { "Invalid login attempt." }
                };
            }

            // Generate token (if needed) or return success
            return new UserResultDto { Success = true};
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }


        public async Task<UserResultDto> ConfirmEmailAsync(string userId, string token)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
                return new UserResultDto { Success = false, Errors = new[] { "Invalid confirmation data" } };

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            token = Uri.UnescapeDataString(token);

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Email Confirmation Failed" } };
        }

        public async Task<UserResultDto> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = $"https://localhost:7003/api/auth/reset-password?email={email}&token={Uri.EscapeDataString(token)}";

        
            return new UserResultDto { Success = true };
        }

        public async Task<UserResultDto> ResetPasswordAsync(ResetPassDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.email);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var result = await _userManager.ResetPasswordAsync(user, dto.token, dto.newPassword);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Failed to reset password" } };
        }

        public async Task<UserResultDto> ChangePasswordAsync(ChangePassDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userId);
            if (user == null) return new UserResultDto { Success = false, Errors = new[] { "User not found" } };

            var result = await _userManager.ChangePasswordAsync(user, dto.currentPassword, dto.newPassword);
            return result.Succeeded
                ? new UserResultDto { Success = true }
                : new UserResultDto { Success = false, Errors = new[] { "Failed to change password" } };
        }


        public async Task<UserResultDto> UpdateUserAsync(EditUserDto user)
        {
            var currentUser = await _userManager.FindByIdAsync(user.Id);

            if (currentUser == null)
            {
                return new UserResultDto
                {
                    Success = false,
                    Message = "User Not Found"
                };
            }

            // فقط عدّل الإيميل إذا تغيّر
            if (!string.Equals(currentUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailOwner = await _userManager.FindByEmailAsync(user.Email);
                if (emailOwner != null && emailOwner.Id != currentUser.Id)
                {
                    return new UserResultDto
                    {
                        Success = false,
                        Message = "Email is already in use by another user."
                    };
                }

                currentUser.Email = user.Email;
                currentUser.UserName = user.Email; // لو تريد توحيد ال Username مع الايميل
            }

            currentUser.FirstName = user.FirstName;
            currentUser.LastName = user.LastName;
            currentUser.PhoneNumber = user.Phone;

            var result = await _userManager.UpdateAsync(currentUser);

            return new UserResultDto
            {
                Success = result.Succeeded,
                Errors = result.Errors?.Select(e => e.Description)!
            };
        }



        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var Roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = Roles.FirstOrDefault()!,
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = _userManager.Users;
            return users.Select(u => new UserDto
            {
                Id = Guid.Parse(u.Id),
                Email = u.Email!,
            });
        }

        public Guid GetLoggedInUser()
        {      
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(userId!);
        }


        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;

           var Roles = await _userManager.GetRolesAsync(user);


            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                Email = user.Email!,
                Role = Roles.FirstOrDefault()!
            };
        }

        public async Task<(Claim[], UserDto)> GetClaims(string email)
        {
            var user = await GetUserByEmailAsync(email);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            return (claims, user);
        }

        public async Task<Claim[]> GetClaimsById(string userId)
        {
            var user = await GetUserByIdAsync(userId);

            var claims = new[] {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            return claims;
        }



    }
}
