using BL.DTOs;
using DAL.Model;
using Domains;

namespace BL.Contracts.Shippment
{
    public interface IUserShippment
    {
        Task<List<ShippmentDto>> GetUserShipments();
        Task<PagedResult<ShippmentDto>> GetUserShipments(int PageNumber);
        Task<ShippmentDto> GetUserShipmentById(Guid Id);
    }




}
