using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class ShippingTypeService : BaseService<TbShippingType,ShippingTypeDto>, IShippingType
    {
        public ShippingTypeService(IGenericRepository<TbShippingType> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
        }
    }




}
