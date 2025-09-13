using BL.Contracts;
using BL.DTOs;
using DAL.Exeptions;
using Domains;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingPackagesController : ControllerBase
    {
        private readonly IShippingPackag _shippingPackageService;

        public ShippingPackagesController(IShippingPackag shippingPackageService)
        {
            _shippingPackageService = shippingPackageService;
        }

        /// <summary>
        /// Get all shipping packages
        /// </summary>
        /// <returns>List of ShippingPackageDto wrapped in ApiResponse</returns>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<ShippingPackageDto>>> GetAll()
        {
            try
            {
                var shippingPackages = _shippingPackageService.GetAll();
                return Ok(ApiResponse<List<ShippingPackageDto>>.SuccessResponse(shippingPackages.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<ShippingPackageDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<ShippingPackageDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get a shipping package by Id
        /// </summary>
        /// <param name="Id">Shipping package Id (Guid)</param>
        /// <returns>Single ShippingPackageDto wrapped in ApiResponse</returns>
        [HttpGet("GetById/{Id}")]
        public ActionResult<ApiResponse<ShippingPackageDto>> GetById(Guid Id)
        {
            try
            {
                var data = _shippingPackageService.GetByID(Id);
                return Ok(ApiResponse<ShippingPackageDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<ShippingPackageDto>.FailResponse("Data Access Exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShippingPackageDto>.FailResponse("General Exception", new List<string>() { ex.Message }));
            }
        }

        /// <summary>
        /// Add new shipping package
        /// </summary>
        /// <param name="shippingPackageDto">ShippingPackageDto object</param>
        /// <returns>Created ShippingPackageDto wrapped in ApiResponse</returns>
        [HttpPost("Add")]
        public ActionResult<ApiResponse<ShippingPackageDto>> Add([FromBody] ShippingPackageDto shippingPackageDto)
        {
            try
            {
                var success = _shippingPackageService.Insert(shippingPackageDto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));


                 shippingPackageDto.Id = newId;
                return Ok(ApiResponse<ShippingPackageDto>.SuccessResponse(shippingPackageDto, "Shipping Package Added Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<ShippingPackageDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippingPackageDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update existing shipping package
        /// </summary>
        /// <param name="Id">Shipping package Id (Guid)</param>
        /// <param name="shippingPackageDto">Updated ShippingPackageDto object</param>
        /// <returns>Updated ShippingPackageDto wrapped in ApiResponse</returns>
        [HttpPut("Update/{Id}")]
        public ActionResult<ApiResponse<ShippingPackageDto>> Update(Guid Id, [FromBody] ShippingPackageDto shippingPackageDto)
        {
            try
            {
                shippingPackageDto.Id = Id;
                var success = _shippingPackageService.Update(shippingPackageDto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));
                
                return Ok(ApiResponse<ShippingPackageDto>.SuccessResponse(shippingPackageDto, "Shipping Package Updated Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<ShippingPackageDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippingPackageDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Change status of a shipping package (e.g., Active/Inactive)
        /// </summary>
        /// <param name="Id">Shipping package Id (Guid)</param>
        /// <returns>Boolean result wrapped in ApiResponse</returns>
        [HttpPatch("ChangeStatus/{Id}")]
        public ActionResult<ApiResponse<bool>> ChangeStatus(Guid Id)
        {
            try
            {
                var result = _shippingPackageService.ChangeStatus(Id, Guid.Empty);
                if (result)
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "Shipping Package Status Changed Successfully"));
                else
                    return NotFound(ApiResponse<bool>.FailResponse("Shipping Package Not Found"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<bool>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }
    }



}
