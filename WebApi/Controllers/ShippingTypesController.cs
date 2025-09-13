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
    public class ShippingTypesController : ControllerBase
    {
        private readonly IShippingType _shippingTypeService;

        public ShippingTypesController(IShippingType shippingTypeService)
        {
            _shippingTypeService = shippingTypeService;
        }

        /// <summary>
        /// Get all shipping types
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<ShippingTypeDto>>> GetAll()
        {
            try
            {
                var shippingTypes = _shippingTypeService.GetAll();
                return Ok(ApiResponse<List<ShippingTypeDto>>.SuccessResponse(shippingTypes.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<ShippingTypeDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<ShippingTypeDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get a shipping type by Id
        /// </summary>
        [HttpGet("GetById/{Id}")]
        public ActionResult<ApiResponse<ShippingTypeDto>> GetById(Guid Id)
        {
            try
            {
                var data = _shippingTypeService.GetByID(Id);
                return Ok(ApiResponse<ShippingTypeDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<ShippingTypeDto>.FailResponse("Data Access Exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<ShippingTypeDto>.FailResponse("General Exception", new List<string>() { ex.Message }));
            }
        }

        /// <summary>
        /// Insert new shipping type
        /// </summary>
        [HttpPost("Insert")]
        public ActionResult<ApiResponse<ShippingTypeDto>> Insert([FromBody] ShippingTypeDto shippingTypeDto)
        {
            try
            {
                var success = _shippingTypeService.Insert(shippingTypeDto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));

                shippingTypeDto.Id = newId;
                return Ok(ApiResponse<ShippingTypeDto>.SuccessResponse(shippingTypeDto, "Shipping Type Added Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<ShippingTypeDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippingTypeDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update existing shipping type
        /// </summary>
        [HttpPut("Update/{Id}")]
        public ActionResult<ApiResponse<ShippingTypeDto>> Update(Guid Id, [FromBody] ShippingTypeDto shippingTypeDto)
        {
            try
            {
                shippingTypeDto.Id = Id; 
                var success = _shippingTypeService.Update(shippingTypeDto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));

                return Ok(ApiResponse<ShippingTypeDto>.SuccessResponse(shippingTypeDto, "Shipping Type Updated Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<ShippingTypeDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippingTypeDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Change status of a shipping type
        /// </summary>
        [HttpPatch("ChangeStatus/{Id}")]
        public ActionResult<ApiResponse<bool>> ChangeStatus(Guid Id)
        {
            try
            {
                var result = _shippingTypeService.ChangeStatus(Id,Guid.Empty);
                if (result)
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "Shipping Type Status Changed Successfully"));
                else
                    return NotFound(ApiResponse<bool>.FailResponse("Shipping Type Not Found"));
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
