using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class CountryService : BaseService<TbCountry,CountryDto>, ICountry
    {
        public CountryService(IGenericRepository<TbCountry> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
        }
    }




}
