using BL.Contracts;
using BL.DTOs;
using BL.Services;
using DAL.Exeptions;
using Domains;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountry _countryService;

        public CountriesController(ICountry countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<CountryDto>>> GetAll()
        {
            try
            {
                var countries = _countryService.GetAll();
                return Ok(ApiResponse<List<CountryDto>>.SuccessResponse(countries.ToList(), "Request Success"));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<CountryDto>>.FailResponse("Data Access Error", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<CountryDto>>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }


        [HttpGet("GetById/{Id}")]
        public ActionResult<ApiResponse<CountryDto>> GetById(Guid Id)
        {
            try
            {
  
                var data = _countryService.GetByID(Id);
                if (data == null)
                    return Ok(ApiResponse<CountryDto>.SuccessMessage($"Country Wiht ID {Id} Not Found !"));

                return Ok(ApiResponse<CountryDto>.SuccessResponse(data));
            }
            catch (DataAccessException daEx)
            {
                return StatusCode(500, ApiResponse<CountryDto>.FailResponse("data access exception", new List<string>() { daEx.Message }));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<CountryDto>.FailResponse("general exception", new List<string>() { ex.Message }));
            }
        }


        [HttpPost("Add")]
        public ActionResult<ApiResponse<CountryDto>> Add([FromBody] CountryDto countryDto)
        {
            try
            {
                var success = _countryService.Insert(countryDto, out Guid newId);

                if (!success)
                    return BadRequest(ApiResponse<Guid>.FailResponse("Insert failed"));


                countryDto.Id = newId;
                return Ok(ApiResponse<CountryDto>.SuccessResponse(countryDto, "Country Added Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<CountryDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CountryDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }


        [HttpPut("Update/{Id}")]
        public ActionResult<ApiResponse<CountryDto>> Update(Guid Id, [FromBody] CountryDto countryDto)
        {
            try
            {
                countryDto.Id = Id; // Ensure consistency
                var success = _countryService.Update(countryDto);

                if (!success)
                    return NotFound(ApiResponse<bool>.FailResponse("Update failed, record not found"));

                return Ok(ApiResponse<CountryDto>.SuccessResponse(countryDto, "Country Updated Successfully"));
            }
            catch (DataAccessException daEx)
            {
                return BadRequest(ApiResponse<CountryDto>.FailResponse("Data Access Error", new List<string> { daEx.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CountryDto>.FailResponse("General Exception Error", new List<string> { ex.Message }));
            }
        }


        [HttpDelete("Delete/{Id}")]
        public ActionResult<ApiResponse<bool>> Delete(Guid Id)
        {
            try
            {
                var isDeleted = _countryService.ChangeStatus(Id,Guid.Empty);
                if (isDeleted)
                    return Ok(ApiResponse<bool>.SuccessResponse(true, "Country Deleted Successfully"));
                else
                    return NotFound(ApiResponse<bool>.FailResponse("Country Not Found"));
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
