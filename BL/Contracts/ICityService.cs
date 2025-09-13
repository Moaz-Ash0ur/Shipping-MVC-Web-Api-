using BL.DTOs;
using Domains;
using System.Linq.Expressions;

namespace BL.Contracts
{
    public interface ICity : IBaseService<TbCity,CityDto>
    {

        //if you want to add new function here
        public List<CityDto> GetAllCities();

        public List<CityDto> GetByCountry(Guid countryId);

    }




}
