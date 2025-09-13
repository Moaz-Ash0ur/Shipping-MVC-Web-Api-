using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class SubscriptionPackageService : BaseService<TbSubscriptionPackage,SubscriptionPackageDto>, ISubscriptionPackage
    {
        public SubscriptionPackageService(IGenericRepository<TbSubscriptionPackage> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {

        }
    }




}
