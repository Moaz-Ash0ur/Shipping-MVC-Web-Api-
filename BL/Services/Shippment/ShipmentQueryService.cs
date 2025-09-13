using AutoMapper;
using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using DAL.Contracts;
using DAL.Model;
using Domains;

namespace BL.Services.Shippment
{
    public class ShipmentQueryService : IShipmentQuery
    {
        private readonly ISpRepository _spRepository;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
         
        public ShipmentQueryService(ISpRepository spRepository, IMapper mapper, IUserService userService)
        {
            _spRepository = spRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<PagedResult<ShippmentDto>> GetUserShipments(int PageNumber,int PageSize)
        {

            Guid userId = (Guid)(_userService.GetLoggedInUser());

            var flats = await _spRepository.ExecuteAsync<ShipmentFlat>
                        ("EXEC dbo.GetShipmentByUser @UserId={0}", userId);

            var ShippmentDto = _mapper.Map<List<ShipmentFlat>, List<ShippmentDto>>(flats);

            return ShippmentDto.ToPagedResult(PageNumber, 10);

        }

        public async Task<ShippmentDto> GetUserShipmentById(Guid Id)
        {

            Guid userId = (Guid)(_userService.GetLoggedInUser());

            var flats = await _spRepository.ExecuteAsync<ShipmentFlat>
                        ("EXEC dbo.GetShipmentByUser @UserId={0}", userId);

            if (flats == null)
                return null;

            var shipment = flats.FirstOrDefault(s => s.Id == Id);

            return _mapper.Map<ShipmentFlat, ShippmentDto>(shipment!);

        }


        //--------------For Admin Panel Managing----------------
        public async Task<PagedResult<ShippmentDto>> GetAllShipments(int PageNumber, int? ShipmentState = null)
        {

            var allShipments = await _spRepository.ExecuteAsync<ShipmentFlat>
                        ("EXEC dbo.GetAllShipments");

            List<ShipmentFlat> filterdShipment = new();

            if (ShipmentState == null)
            {
                filterdShipment = allShipments;
            }
            else
            {
                filterdShipment = allShipments.Where(s => s.CurrentState == ShipmentState).ToList();
            }

            //make filter here
            var ShippmentDto = _mapper.Map<List<ShipmentFlat>, List<ShippmentDto>>(filterdShipment);

            return ShippmentDto.ToPagedResult(PageNumber, 10);

        }

        public async Task<ShippmentDto> GetAllShipmentsById(Guid ShipmentId , int? ShipmentState = null)
        {

            var allShipments = await _spRepository.ExecuteAsync<ShipmentFlat>
                        ("EXEC dbo.GetAllShipments");

             ShipmentFlat filterdShipment = new();

            if (ShipmentState == null)
            {
                filterdShipment = allShipments.FirstOrDefault(s => s.Id == ShipmentId);
            }
            else
            {
                filterdShipment = allShipments.Where(s => s.CurrentState == ShipmentState).FirstOrDefault(s => s.Id == ShipmentId);
            }

            var ShippmentDto = _mapper.Map<ShippmentDto>(filterdShipment);

            return ShippmentDto;

        }



    }



}
