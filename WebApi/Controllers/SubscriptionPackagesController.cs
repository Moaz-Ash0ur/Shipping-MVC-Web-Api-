using BL.Contracts;
using BL.DTOs;
using BL.Services;
using DAL.Exeptions;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriptionPackageController : ControllerBase
    {
        private readonly ISubscriptionPackage _subscriptionPackageService;

        public SubscriptionPackageController(ISubscriptionPackage subscriptionPackageService)
        {
            _subscriptionPackageService = subscriptionPackageService;
        }

        /// <summary>
        /// Get all subscription packages
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<SubscriptionPackageDto>>> GetAll()
        {
            try
            {
                var packages = _subscriptionPackageService.GetAll();
                return Ok(ApiResponse<List<SubscriptionPackageDto>>.SuccessResponse(packages.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<SubscriptionPackageDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<SubscriptionPackageDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get subscription package by Id
        /// </summary>
        [HttpGet("GetById/{id}")]
        public ActionResult<ApiResponse<SubscriptionPackageDto>> GetById(Guid id)
        {
            try
            {
                var data = _subscriptionPackageService.GetByID(id);
                return Ok(ApiResponse<SubscriptionPackageDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Add new subscription package
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<SubscriptionPackageDto>> Add([FromBody] SubscriptionPackageDto dto)
        {
            try
            {
                Guid ID = Guid.Empty;
                var created = _subscriptionPackageService.Insert(dto,out ID);
                if (created)
                {
                    dto.Id = ID;
                    return CreatedAtAction(nameof(GetById), new { id = ID }, ApiResponse<SubscriptionPackageDto>.SuccessResponse(dto, "Created successfully"));
                }

                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("Data access exception", new List<string> { "Faild add Package" }));
            }        
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }

        }

        /// <summary>
        /// Update existing subscription package
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<ApiResponse<SubscriptionPackageDto>> Update(Guid id, [FromBody] SubscriptionPackageDto dto)
        {
            try
            {
                dto.Id = id; 
                var updated = _subscriptionPackageService.Update(dto);
                return Ok(ApiResponse<SubscriptionPackageDto>.SuccessResponse(dto, "Updated successfully"));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<SubscriptionPackageDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete subscription package (soft delete by changing status)
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var result = _subscriptionPackageService.ChangeStatus(id, Guid.Empty);
                return Ok(ApiResponse<bool>.SuccessResponse(result, "Deleted successfully"));
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
