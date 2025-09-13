using BL.DTOs;
using BL.Services.Shippment.ShipmentStatus;
using Domains;

namespace BL.Contracts.Shippment
{
    public interface IShipmentStatus : IBaseService<TbShippmentStatus,ShippmentStatusDto>
    {
        void InsertShipmentState(Guid ShipmentId, ShipmentStatusEnum status);
    }


}
