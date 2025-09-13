using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using BL.DTOs.Base;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class ShippingPackageService : BaseService<TbShippingPackage, ShippingPackageDto>, IShippingPackag
    {
        public ShippingPackageService(IGenericRepository<TbShippingPackage> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
        }
    }




}
