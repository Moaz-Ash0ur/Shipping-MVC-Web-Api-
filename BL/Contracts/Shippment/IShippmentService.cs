using BL.DTOs;
using BL.Services;
using BL.Services.Shippment;
using DAL.Model;
using Domains;

namespace BL.Contracts.Shippment
{
    public interface IShippment : IBaseService<TbShippment,ShippmentDto>
    {
        Task  CreateAsync(ShippmentDto shippmentDto);
        Task UpdateAsync(ShippmentDto shippmentDto);
   


    }




}
