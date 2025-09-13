using BL.DTOs;
using BL.Services;
using DAL.Model;
using Domains;

namespace BL.Contracts.Shippment
{
    public interface IShipmentQuery
    {
       Task<PagedResult<ShippmentDto>> GetUserShipments(int PageNumber, int PageSize);
       Task<ShippmentDto> GetUserShipmentById(Guid Id);
       Task<PagedResult<ShippmentDto>> GetAllShipments(int PageNumber, int? ShipmentState = null);
       Task<ShippmentDto> GetAllShipmentsById(Guid ShipmentId, int? ShipmentState = null);

    }





}
