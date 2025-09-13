using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class PaymentMethodService : BaseService<TbPaymentMethod,PaymentMethodDto>, IPaymentMethodService
    {
        public PaymentMethodService(IGenericRepository<TbPaymentMethod> repo, IMapper mapper, IUserService userService) : base(repo, mapper, userService)
        {
        }
    }




}
