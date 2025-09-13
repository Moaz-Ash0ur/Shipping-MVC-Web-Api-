using BL.Contracts;
using BL.DTOs;
using DAL.Exeptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    /// <summary>
    /// API Controller responsible for handling user profile and subscription operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserSubscription _userSubscription;
        private readonly IUserService _userService;

        public UserAccountController(IUserSubscription userSubscription, IUserService userService)
        {
            _userSubscription = userSubscription;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves the profile information of the currently logged-in user.
        /// </summary>
        /// <returns>
        /// 200 OK with <see cref="UserDto"/> if found, otherwise a failure response.
        /// </returns>
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = _userService.GetLoggedInUser().ToString();
            var userInfo = await _userService.GetUserByIdAsync(userId);

            if (userInfo != null)
            {
                return Ok(ApiResponse<UserDto>.SuccessResponse(userInfo, "User retrieved successfully."));
            }

            return Ok(ApiResponse<UserDto>.FailResponse("User data not found."));
        }

        /// <summary>
        /// Updates the profile of the currently logged-in user.
        /// </summary>
        /// <param name="userId">
        /// The user ID (ignored, it is overridden by the logged-in user's ID).
        /// </param>
        /// <param name="dto">
        /// The updated user data.
        /// </param>
        /// <returns>
        /// 200 OK with updated <see cref="EditUserDto"/> if successful, otherwise a failure response.
        /// </returns>
        [HttpPut("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfileAsync(Guid userId, [FromBody] EditUserDto dto)
        {
            userId = _userService.GetLoggedInUser();
            dto.Id = userId.ToString();

            var result = await _userService.UpdateUserAsync(dto);

            if (result.Success)
            {
                return Ok(ApiResponse<EditUserDto>.SuccessResponse(dto, "User updated successfully."));
            }

            return Ok(ApiResponse<EditUserDto>.FailResponse("Failed to update user."));
        }

        /// <summary>
        /// Retrieves the subscription details (with packages) for the currently logged-in user.
        /// </summary>
        /// <param name="userId">
        /// The user ID (ignored, it is overridden by the logged-in user's ID).
        /// </param>
        /// <returns>
        /// 200 OK with <see cref="UserSubscriptionDto"/> if subscription exists, otherwise a failure response.
        /// </returns>
        [HttpGet("GetUserSubscription/{userId}")]
        public IActionResult GetUserSubscription(Guid userId)
        {
            userId = _userService.GetLoggedInUser();

            var userInfo = _userSubscription.GetSubscriptionWithPackages(userId);

            if (userInfo != null)
            {
                return Ok(ApiResponse<UserSubscriptionDto>.SuccessResponse(userInfo, "User subscription retrieved successfully."));
            }

            return Ok(ApiResponse<UserSubscriptionDto>.FailResponse("User is not subscribed to any package."));
        }
    }


}
