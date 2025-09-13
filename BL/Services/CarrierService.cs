using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class CarrierService : BaseService<TbCarrier, CarrierDto>, ICarrierService
    {

        public CarrierService(IGenericRepository<TbCarrier> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
           
        }



    }

}
