using AutoMapper;
using BL.Contracts;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class BaseService<T,DTO> : IBaseService<T,DTO> where T : BaseTable
    {           

        private readonly IGenericRepository<T> _repo;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public BaseService(IGenericRepository<T> repo, IMapper mapper, IUserService userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _repo = _unitOfWork.GetRepository<T>();
            _mapper = mapper;
            _userService = userService;
        }


        public IEnumerable<DTO> GetAll()
        {
            var dbObject = _repo.GetAll();

            return _mapper.Map<List<T>, List<DTO>>((List<T>)dbObject);
             
        }

        public DTO GetByID(Guid Id)
        {
            var dbObject = _repo.GetByID(Id);
            return _mapper.Map<T,DTO>(dbObject);
        }

        public bool Insert(DTO entity)
        {
            var dbObject = _mapper.Map<DTO,T>(entity);
            dbObject.CreatedBy = _userService.GetLoggedInUser();
            return _repo.Insert(dbObject);
        }

        public bool Insert(DTO entity, out Guid Id)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);
            dbObject.CreatedBy = _userService.GetLoggedInUser();          
            return _repo.Insert(dbObject,out Id);            
        }

        public bool Update(DTO entity)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);

            dbObject.UpdatedBy = _userService.GetLoggedInUser();
            return _repo.Update(dbObject);
        }

        public bool ChangeStatus(Guid ID, Guid userId, int status = 1)
        {
            return _repo.ChangeStatus(ID,_userService.GetLoggedInUser(),status);
        }




    }


}
