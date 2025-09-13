using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services.Shippment.ShipmentStatus
{
    public class Returned : IShipmentStatusHandler
    {

        private readonly IGenericRepository<TbShippment> _repo;
        private readonly IShipmentStatus _shipmentStatusSrv;
        private readonly IUserService _userService;
        private readonly IShipmentQuery _shipmentQuery;

        public Returned(IGenericRepository<TbShippment> repo, IShipmentStatus shipmentStatusSrv, IUserService userService, IShipmentQuery shipmentQuery)
        {
            _repo = repo;
            _shipmentStatusSrv = shipmentStatusSrv;
            _userService = userService;
            _shipmentQuery = shipmentQuery;
        }


        public ShipmentStatusEnum TargetState => ShipmentStatusEnum.Returned;

        public async Task HandelState(ShippmentDto shippmentDto)
        {
            var shipment = await _shipmentQuery.GetAllShipmentsById(shippmentDto.Id);

            _repo.ChangeStatus(shippmentDto.Id, _userService.GetLoggedInUser(), (int)ShipmentStatusEnum.Returned);

            _shipmentStatusSrv.InsertShipmentState(shippmentDto.Id, ShipmentStatusEnum.Returned);
        }
    }





}
