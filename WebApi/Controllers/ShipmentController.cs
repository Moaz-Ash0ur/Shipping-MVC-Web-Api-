 using BL.Contracts.Shippment;
using BL.DTOs;
using BL.Services.Shippment.ShipmentStatus;
using DAL.Exeptions;
using Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShippment _shipmentService;
        private readonly IShipmentQuery _shipmentQuery;
        private readonly IShipmentStatusFactory _shipmentStatusHandler;

        public ShipmentController(IShippment shipmentService, IShipmentQuery shipmentQuery, IShipmentStatusFactory shipmentStatusHandler)
        {
            _shipmentService = shipmentService;
            _shipmentQuery = shipmentQuery;
            _shipmentStatusHandler = shipmentStatusHandler;
        }

        /// <summary>
        /// Get all shipments (non-paginated).
        /// </summary>
        [HttpGet("GetAll")]
        public ActionResult<ApiResponse<List<ShippmentDto>>> GetAllShipments()
        {
            try
            {
                var shipments = _shipmentService.GetAll();
                return Ok(ApiResponse<List<ShippmentDto>>.SuccessResponse(shipments.ToList(), "Request successful."));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("Data access error.", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("General exception error.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Create a new shipment.
        /// </summary>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateShipmentAsync([FromBody] ShippmentDto data)
        {
            if (data == null)
                return BadRequest(ApiResponse<string>.FailResponse("Shipment data is required."));

            try
            {
                await _shipmentService.CreateAsync(data);
                return Ok(ApiResponse<object>.SuccessResponse(data, "Shipment created successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error creating shipment.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Update an existing shipment.
        /// </summary>
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateShipmentAsync([FromBody] ShippmentDto data)
        {
            if (data == null)
                return BadRequest(ApiResponse<string>.FailResponse("Shipment data is required."));

            try
            {
                await _shipmentService.UpdateAsync(data);
                return Ok(ApiResponse<object>.SuccessResponse(data, "Shipment updated successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error updating shipment.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Delete a shipment by ID.
        /// </summary>
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteShipment(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(ApiResponse<string>.FailResponse("Shipment ID is required."));

            try
            {
                _shipmentService.ChangeStatus(id, Guid.Empty);
                return Ok(ApiResponse<object>.SuccessResponse(new { Message = "Deleted successfully" }, "Shipment deleted successfully."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error deleting shipment.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get shipments for the current user (paginated).
        /// </summary>
        [HttpGet("GetUserShipments/{pageNumber}/{pageSize}")]
        public async Task<ActionResult<ApiResponse<List<ShippmentDto>>>> GetUserShipments(int pageNumber, int pageSize)
        {
            try
            {
                var shipments = await _shipmentQuery.GetUserShipments(pageNumber, pageSize);
                return Ok(ApiResponse<List<ShippmentDto>>.SuccessResponse(shipments.Items, "Shipments retrieved successfully."));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("Data access error.", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("General exception error.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get a single shipment by ID (for user).
        /// </summary>
        [HttpGet("GetUserShipmentById/{id}")]
        public async Task<ActionResult<ApiResponse<ShippmentDto>>> GetUserShipmentById(Guid id)
        {
            try
            {
                var shipment = await _shipmentQuery.GetUserShipmentById(id);
                return Ok(ApiResponse<ShippmentDto>.SuccessResponse(shipment, "Shipment retrieved successfully."));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<ShippmentDto>.FailResponse("Data access error.", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippmentDto>.FailResponse("General exception error.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get all shipments (paginated).
        /// </summary>
        [HttpGet("GetAllShipments/{pageNumber}")]
        public async Task<ActionResult<ApiResponse<List<ShippmentDto>>>> GetAllShipments(int pageNumber)
        {
            try
            {
                var shipments = await _shipmentQuery.GetAllShipments(pageNumber);
                return Ok(ApiResponse<List<ShippmentDto>>.SuccessResponse(shipments.Items, "Shipments retrieved successfully."));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("Data access error.", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<List<ShippmentDto>>.FailResponse("General exception error.", new List<string> { ex.Message }));
            }
        }

        /// <summary>
        /// Get shipment details by ID.
        /// </summary>
        [HttpGet("GetShipmentById/{shipmentId}")]
        public async Task<ActionResult<ApiResponse<ShippmentDto>>> GetShipmentById(Guid shipmentId)
        {
            try
            {
                var shipment = await _shipmentQuery.GetAllShipmentsById(shipmentId);
                return Ok(ApiResponse<ShippmentDto>.SuccessResponse(shipment, "Shipment retrieved successfully."));
            }
            catch (DataAccessException dex)
            {
                return BadRequest(ApiResponse<ShippmentDto>.FailResponse("Data access error.", new List<string> { dex.Message }));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ShippmentDto>.FailResponse("General exception error.", new List<string> { ex.Message }));
            }
        }



        /// <summary>
        /// Change the status of an existing shipment.
        /// </summary>
        /// <param name="data">Shipment data (only Id is required).</param>
        /// <param name="newStatus">
        /// New status to apply to the shipment.
        /// Available values: Created = 0, Deleted = 1, Approved = 2, ReadyForShip = 3, Shipped = 4, Delivered = 5, Returned = 6.
        /// </param>
        /// <returns>Returns the updated shipment with the new status.</returns>
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeShipmentStatus([FromBody] ShippmentDto data,  ShipmentStatusEnum newStatus)
        {
            if (data == null || data.Id == Guid.Empty)
                return BadRequest(ApiResponse<string>.FailResponse("Valid shipment data with ID is required."));

            try
            {
                var statusHandler = _shipmentStatusHandler.GetHandler(newStatus);
                await statusHandler.HandelState(data);

                return Ok(ApiResponse<object>.SuccessResponse(data, $"Shipment status changed to {newStatus}."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.FailResponse("Error changing shipment status.", new List<string> { ex.Message }));
            }
        }





    }



}
