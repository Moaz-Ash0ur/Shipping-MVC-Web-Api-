using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services.Shippment.ShipmentStatus
{
    //This for Use Strategy Pattern is a Behaviar Pattren 
    //have more one logic in same idea to cant repeat your self and apply O/C Principle

    public class ApprovedShipment : IShipmentStatusHandler
    {

        private readonly IGenericRepository<TbShippment> _repo;
        private readonly IShipmentStatus _shipmentStatusSrv;
        private readonly IUserService _userService;
        private readonly IShipmentQuery _shipmentQuery;

        public ApprovedShipment(IGenericRepository<TbShippment> repo, IShipmentStatus shipmentStatusSrv, IUserService userService, IShipmentQuery shipmentQuery)
        {
            _repo = repo;
            _shipmentStatusSrv = shipmentStatusSrv;
            _userService = userService;
            _shipmentQuery = shipmentQuery;
        }

        public ShipmentStatusEnum TargetState => ShipmentStatusEnum.Approved;

        public async Task HandelState(ShippmentDto shippmentDto)
        {
            var shipment = await _shipmentQuery.GetAllShipmentsById(shippmentDto.Id);

            _repo.Update(shippmentDto.Id, s =>
            {
                s.CurrentState = (int)ShipmentStatusEnum.Approved;
                s.UpdatedBy = _userService.GetLoggedInUser();
                s.UpdatedDate = DateTime.Now;
            });


            _shipmentStatusSrv.InsertShipmentState(shippmentDto.Id, ShipmentStatusEnum.Approved);
        }
    }





}
