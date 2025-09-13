using AutoMapper;
using BL.DTOs;
using Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //Domains
            CreateMap<TbShippment, ShippmentDto>().ReverseMap();
            CreateMap<TbShippingType, ShippingTypeDto>().ReverseMap();
            CreateMap<TbCountry, CountryDto>().ReverseMap();
            CreateMap<TbCity, CityDto>().ReverseMap();
            CreateMap<TbUserSender, UserSenderDto>().ReverseMap();
            CreateMap<TbUserReceiver, UserReceiverDto>().ReverseMap();
            CreateMap<TbRefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<TbShippingPackage, ShippingPackageDto>().ReverseMap();
            CreateMap<TbShippmentStatus, ShippmentStatusDto>().ReverseMap();
            CreateMap<TbCarrier, CarrierDto>().ReverseMap();
            CreateMap<TbSubscriptionPackage, SubscriptionPackageDto>().ReverseMap();

           // CreateMap<TbUserSubscription, UserSubscriptionDto>().ReverseMap();


            CreateMap<TbUserSubscription, UserSubscriptionDto>()
           .ForMember(dest => dest.Package,
                      opt => opt.MapFrom(src => src.Package))
           .ReverseMap();








            //Views
            CreateMap<VwCities, CityDto>().ReverseMap();


            //Stored Procedure | Custome Mapping dor member UserSender & Reciver
            CreateMap<ShipmentFlat, ShippmentDto>()
            .ForMember(dest => dest.UserSender, opt => opt.MapFrom(src => new UserSenderDto
            {
                UserId = src.SenderUserId,
                SenderName = src.SenderName,
                Email = src.SenderEmail,
                Phone = src.SenderPhone,
                PostalCode = src.SenderPostalCode,
                Contact = src.SenderContact,
                OtherAddress = src.SenderOtherAddress,
                IsDefault = src.SenderIsDefault,
                CityId = src.SenderCityId,
                CountryId = src.SenderCountryId,
                Address = src.SenderAddress
            }))
            .ForMember(dest => dest.UserReceiver, opt => opt.MapFrom(src => new UserReceiverDto
            {
                UserId = src.ReceiverUserId,
                ReceiverName = src.ReceiverName,
                Email = src.ReceiverEmail,
                Phone = src.ReceiverPhone,
                PostalCode = src.ReceiverPostalCode,
                Contact = src.ReceiverContact,
                OtherAddress = src.ReceiverOtherAddress,
                IsDefault = src.ReceiverIsDefault,
                CityId = src.ReceiverCityId,
                CountryId = src.ReceiverCountryId,
                Address = src.ReceiverAddress
            }));



        }



    }


}
