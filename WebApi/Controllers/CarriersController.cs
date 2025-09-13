using BL.Contracts;
using BL.DTOs;
using BL.Services;
using DAL.Exeptions;
using Domains;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Models;

namespace WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CarriersController : ControllerBase
    {

        private readonly ICarrierService _carrierService;

        public CarriersController(ICarrierService carrierService)
        {
            _carrierService = carrierService;
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<CarrierDto>>> GetAll()
        {
            try
            {
                var cities = _carrierService.GetAll();
                return Ok(ApiResponse<List<CarrierDto>>.SuccessResponse(cities.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<CarrierDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<CarrierDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get city by Id
        /// </summary>
        [HttpGet("GetById/{id}")]
        public ActionResult<ApiResponse<CarrierDto>> GetById(Guid id)
        {
            try
            {
                var data = _carrierService.GetByID(id);
                return Ok(ApiResponse<CarrierDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<CarrierDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CarrierDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Add new city
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<Guid>> Add([FromBody] CarrierDto cityDto)
        {
            try
            {
                var success = _carrierService.Insert(cityDto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));

                return CreatedAtAction(nameof(GetById), new { id = newId }, ApiResponse<Guid>.SuccessResponse(newId, "Carrier created successfully"));
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
        /// Update city
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CarrierDto cityDto)
        {
            try
            {
                cityDto.Id = id; 
                var success = _carrierService.Update(cityDto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Carrier updated successfully"));
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
        /// Delete (soft delete) city
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                var success = _carrierService.ChangeStatus(id, Guid.Empty);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Delete failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "Carrier deleted successfully"));
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
