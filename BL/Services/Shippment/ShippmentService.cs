using AutoMapper;
using BL.Contracts;
using BL.Contracts.Shippment;
using BL.DTOs;
using BL.Services.Shippment.ShipmentStatus;
using DAL.Contracts;
using DAL.Model;
using DAL.Repositoreis;
using Domains;

namespace BL.Services.Shippment
{
    public class ShippmentService : BaseService<TbShippment,ShippmentDto> , IShippment
    {
        
        private readonly IUserSender _userSender;
        private readonly IUserReceiver _userReceiver;
        private readonly ITrackingNumberCreater _trackingNumberCreater;
        private readonly IRateCalculater _rateCalculater;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        private readonly IUserSubscription _userSubscription;
        private readonly IShipmentStatus _shipmentStatusSrv;
        private readonly IShipmentQuery _shipmentQuery;




        public ShippmentService(IGenericRepository<TbShippment> repo, IUnitOfWork unitOfWork, IMapper mapper, IUserService userService,
             IUserSender userSender, IUserReceiver userReceiver
            , ITrackingNumberCreater trackingNumberCreater, IRateCalculater rateCalculater, ISpRepository spRepository, IShipmentStatus shipmentStatusSrv, IShipmentQuery shipmentQuery, IUserSubscription userSubscription)
            : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
            _userSender = userSender;
            _userReceiver = userReceiver;
            _trackingNumberCreater = trackingNumberCreater;
            _rateCalculater = rateCalculater;
            _userService = userService;
            _shipmentStatusSrv = shipmentStatusSrv;
            _shipmentQuery = shipmentQuery;
            _userSubscription = userSubscription;
        }


        public async Task CreateAsync(ShippmentDto shippmentDto)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var userIdLoggedIn = _userService.GetLoggedInUser();

                shippmentDto.TrackingNumber = _trackingNumberCreater.Create();

                shippmentDto.ShippingRate = _rateCalculater.Calculate(shippmentDto);


                if (_userSubscription.HaveSubscription(userIdLoggedIn))
                {
                    shippmentDto.UserSubscriptionId = userIdLoggedIn;
                    _userSubscription.DeductSubscription(userIdLoggedIn, shippmentDto.Weight , 12);
                }

                // Check if sender exists
                if (shippmentDto.SenderId == Guid.Empty && shippmentDto.UserSender != null)
                {
                    var gUserSenderId = Guid.Empty;
                    shippmentDto.UserSender.UserId = userIdLoggedIn;
                    _userSender.Insert(shippmentDto.UserSender, out gUserSenderId);
                    shippmentDto.SenderId = gUserSenderId;
                }

                // Check if receiver exists
                if (shippmentDto.ReceiverId == Guid.Empty && shippmentDto.UserReceiver != null)
                {
                    var gUserReceiverId = Guid.Empty;
                    shippmentDto.UserReceiver.UserId = userIdLoggedIn;
                    _userReceiver.Insert(shippmentDto.UserReceiver, out gUserReceiverId);
                    shippmentDto.ReceiverId = gUserReceiverId;
                }

                Guid ShipmentId = Guid.Empty;
                this.Insert(shippmentDto, out ShipmentId);

                _shipmentStatusSrv.InsertShipmentState(ShipmentId, ShipmentStatusEnum.Created);

                await _unitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();              
            }
            
        }

        private void FillOldShipmetnInfo(ShippmentDto dto, ShippmentDto oldShipment)
        {
            dto.SenderId = oldShipment.SenderId;
            dto.ReceiverId = oldShipment.ReceiverId;

            dto.UserSender.Id = oldShipment.SenderId;
            dto.UserSender.UserId = oldShipment.UserSender.UserId;
            dto.UserSender.Email = oldShipment.UserSender.Email;
            dto.UserSender.Contact = oldShipment.UserSender.Contact;

            dto.UserReceiver.Id = oldShipment.ReceiverId;
            dto.UserReceiver.UserId = oldShipment.UserReceiver.UserId;
            dto.UserReceiver.Email = oldShipment.UserReceiver.Email;
            dto.UserReceiver.Contact = oldShipment.UserReceiver.Contact;
            dto.UserReceiver.OtherAddress = oldShipment.UserReceiver.OtherAddress;

        }

        public async Task UpdateAsync(ShippmentDto shippmentDto)
        {

            var oldShipment = await _shipmentQuery.GetUserShipmentById(shippmentDto.Id);
            if (oldShipment == null) return;

            try
            {
                await _unitOfWork.BeginTransaction();

                FillOldShipmetnInfo(shippmentDto,oldShipment);

                if (shippmentDto.UserSender.Id != Guid.Empty)
                {
                    _userSender.Update(shippmentDto.UserSender);
                }

                if (shippmentDto.UserReceiver.Id != Guid.Empty)
                {
                    _userReceiver.Update(shippmentDto.UserReceiver);
                }

                this.Update(shippmentDto);
                await _unitOfWork.CommitChanges();
            }
            catch (Exception ex)
            {
                await _unitOfWork.Rollback();
            }

        }









    }



}
