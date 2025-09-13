using AutoMapper;
using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services.Shippment.ShipmentStatus
{
    public class ShippmentStatusService : BaseService<TbShippmentStatus, ShippmentStatusDto>, IShipmentStatus
    {
        
        public ShippmentStatusService(IGenericRepository<TbShippmentStatus> repo, IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
            : base(unitOfWork, mapper, userService)
        {
     
        }

        public void InsertShipmentState(Guid ShipmentId, ShipmentStatusEnum status)
        {
            ShippmentStatusDto shippmentStatusDto = new ShippmentStatusDto();
            shippmentStatusDto.ShippmentId = ShipmentId;
            shippmentStatusDto.CurrentState = (int)status;

            Insert(shippmentStatusDto);
        }

    }


}
