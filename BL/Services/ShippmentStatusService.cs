using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class ShippmentStatusService : BaseService<TbShippmentStatus,ShippmentStatusDto>, IShippmentStatusService
    {
        public ShippmentStatusService(IGenericRepository<TbShippmentStatus> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
        }
    }




}
