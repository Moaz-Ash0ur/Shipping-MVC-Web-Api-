using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using DAL.Repositoreis;
using Domains;

namespace BL.Services
{
    public class UserSenderService : BaseService<TbUserSender,UserSenderDto>, IUserSender
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserSenderService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
        }
    }




}
