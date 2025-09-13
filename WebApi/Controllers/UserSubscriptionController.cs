using BL.Contracts;
using BL.DTOs;
using DAL.Exeptions;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserSubscriptionController : ControllerBase
    {

        private readonly IUserSubscription _userSubscription;

        public UserSubscriptionController(IUserSubscription userSubscription)
        {
            _userSubscription = userSubscription;
        }

        /// <summary>
        /// Get all user subscriptions
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<UserSubscriptionDto>>> GetAll()
        {
            try
            {
                var subscriptions = _userSubscription.GetAll();
                return Ok(ApiResponse<List<UserSubscriptionDto>>.SuccessResponse(subscriptions.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<UserSubscriptionDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<UserSubscriptionDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get user subscription by Id
        /// </summary>
        [HttpGet("GetById/{id}")]
        public ActionResult<ApiResponse<UserSubscriptionDto>> GetById(Guid id)
        {
            try
            {
                var data = _userSubscription.GetByID(id);
                return Ok(ApiResponse<UserSubscriptionDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }


        /// <summary>
        /// Get user subscription by User Id
        /// </summary>
        [HttpGet("GetByUser/{UserId}")]
        public ActionResult<ApiResponse<UserSubscriptionDto>> GetByUser(Guid UserId)
        {
            try
            {
                var data = _userSubscription.GetByUser(UserId);
                if(data != null)
                return Ok(ApiResponse<UserSubscriptionDto>.SuccessResponse(data,"Data Retreived Successfully"));

                return Ok(ApiResponse<UserSubscriptionDto>.SuccessMessage($"User Subscription Wiht User Id {UserId} Not Found"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }


        /// <summary>
        /// Get user subscription by Package Id
        /// </summary>
        [HttpGet("GetByPackage/{PackageId}")]
        public ActionResult<ApiResponse<UserSubscriptionDto>> GetByPackage(Guid PackageId)
        {
            try
            {
                var data = _userSubscription.GetByPackage(PackageId);
                return Ok(ApiResponse<UserSubscriptionDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<UserSubscriptionDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }


        /// <summary>
        /// user subscription
        /// </summary>
        [HttpPost("Subscribe")]
        public ActionResult<ApiResponse<Guid>> Subscribe([FromBody] UserSubscriptionDto dto)
        {
            try
            {
                var success = _userSubscription.Insert(dto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));

                return CreatedAtAction(nameof(GetById), new { id = newId }, ApiResponse<Guid>.SuccessResponse(newId, "Created successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<Guid>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Guid>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update user subscription
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<bool>> Update(Guid id, [FromBody] UserSubscriptionDto dto)
        {
            try
            {
                dto.Id = id; // Ensure consistency
                var success = _userSubscription.Update(dto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Updated successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete (soft delete) user subscription
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var success = _userSubscription.ChangeStatus(id, Guid.Empty);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Delete failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Deleted successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }
    }



}
