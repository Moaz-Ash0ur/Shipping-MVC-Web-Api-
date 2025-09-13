using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services.Shippment.ShipmentStatus
{
    public class ReadyForShip : IShipmentStatusHandler
    {

        private readonly IGenericRepository<TbShippment> _repo;
        private readonly IShipmentStatus _shipmentStatusSrv;
        private readonly IUserService _userService;
        private readonly IShipmentQuery _shipmentQuery;

        public ReadyForShip(IGenericRepository<TbShippment> repo, IShipmentStatus shipmentStatusSrv, IUserService userService, IShipmentQuery shipmentQuery)
        {
            _repo = repo;
            _shipmentStatusSrv = shipmentStatusSrv;
            _userService = userService;
            _shipmentQuery = shipmentQuery;
        }

        public ShipmentStatusEnum TargetState => ShipmentStatusEnum.ReadyForShip;

        public async Task HandelState(ShippmentDto shippmentDto)
        {
            var shipment = await _shipmentQuery.GetAllShipmentsById(shippmentDto.Id);

            _repo.Update(shippmentDto.Id, s =>
            {
                s.CurrentState = (int)ShipmentStatusEnum.ReadyForShip;
                s.UpdatedBy = _userService.GetLoggedInUser();
                s.UpdatedDate = DateTime.Now;
                s.CarrierId = shippmentDto.CarrierId;
            });

            _shipmentStatusSrv.InsertShipmentState(shippmentDto.Id, ShipmentStatusEnum.ReadyForShip);
        }
    }





}
