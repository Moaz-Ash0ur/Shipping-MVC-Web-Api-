using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class UserReceiverService : BaseService<TbUserReceiver,UserReceiverDto>, IUserReceiver
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserReceiverService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService) : base(unitOfWork, mapper, userService)
        {
            _unitOfWork = unitOfWork;
        }
    }




}
