using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class CityService : BaseService<TbCity,CityDto>, ICity
    {
        IViewRepository<VwCities> _Vwrepo;
        IMapper _mapper;

        public CityService(IGenericRepository<TbCity> repo, IMapper mapper, IUserService userService, IViewRepository<VwCities> vwrepo) : base(repo, mapper, userService)
        {
            _Vwrepo = vwrepo;
            _mapper = mapper;
        }

        public List<CityDto> GetAllCities()
        {
           var cities = _Vwrepo.GetAll().ToList();     
           return _mapper.Map<List<VwCities>, List<CityDto>>(cities);
        }

        public List<CityDto> GetByCountry(Guid countryId)
        {
            var cities = _Vwrepo.GetList(a => a.CurrentState != 1 && a.CountryId == countryId).ToList();
            return _mapper.Map<List<VwCities>, List<CityDto>>(cities);
        }

    }

}
