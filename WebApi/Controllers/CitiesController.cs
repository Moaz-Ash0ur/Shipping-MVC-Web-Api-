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
    public class CityController : ControllerBase
    {
        private readonly ICity _cityService;

        public CityController(ICity cityService)
        {
            _cityService = cityService;
        }

        /// <summary>
        /// Get all cities
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<CityDto>>> GetAll()
        {
            try
            {
                var cities = _cityService.GetAllCities();
                return Ok(ApiResponse<List<CityDto>>.SuccessResponse(cities.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<CityDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<CityDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get city by Id
        /// </summary>
        [HttpGet("GetById/{id}")]
        public ActionResult<ApiResponse<CityDto>> GetById(Guid id)
        {
            try
            {
                var data = _cityService.GetByID(id);
                if (data == null)
                    return Ok(ApiResponse<CityDto>.SuccessMessage($"City Wiht ID {id} Not Found !"));

                return Ok(ApiResponse<CityDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<CityDto>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CityDto>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get all cities by country Id
        /// </summary>
        [HttpGet("GetByCountryId/{id}")]
        public ActionResult<ApiResponse<List<CityDto>>> GetByCountryId(Guid id)
        {
            try
            {
                var data = _cityService.GetByCountry(id);
                return Ok(ApiResponse<List<CityDto>>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse("Data access exception", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<CityDto>>.FailResponse("General exception", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Add new city
        /// </summary>
        [HttpPost]
        public ActionResult<ApiResponse<Guid>> Add([FromBody] CityDto cityDto)
        {
            try
            {
                var success = _cityService.Insert(cityDto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));

                return CreatedAtAction(nameof(GetById), new { id = newId }, ApiResponse<Guid>.SuccessResponse(newId, "City created successfully"));
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
        public ActionResult<ApiResponse<bool>> Update(Guid id, [FromBody] CityDto cityDto)
        {
            try
            {
                cityDto.Id = id; // Ensure consistency
                var success = _cityService.Update(cityDto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "City updated successfully"));
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
        public ActionResult<ApiResponse<bool>> Delete(Guid id)
        {
            try
            {
                var success = _cityService.ChangeStatus(id, Guid.Empty);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Delete failed, record not found"));

                return Ok(ApiResponse<bool>.SuccessResponse(true, "City deleted successfully"));
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
