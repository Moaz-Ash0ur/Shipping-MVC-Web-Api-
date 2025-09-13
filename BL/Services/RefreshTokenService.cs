using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{

    public class RefreshTokenService : BaseService<TbRefreshToken, RefreshTokenDto>, IRefreshTokens
    {

        private IGenericRepository<TbRefreshToken> _repo;
        private IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IRefreshTokensRetrival _refreshTokensRetrival;

        public RefreshTokenService(IGenericRepository<TbRefreshToken> repo, IMapper mapper,
            IUserService userService, IRefreshTokensRetrival refreshTokensRetrival) : base(repo, mapper, userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
            _refreshTokensRetrival = refreshTokensRetrival;
        }


        public bool Refresh(RefreshTokenDto tokenDto)
        {        
            var allTokens = _repo.GetList(r => r.UserId == Guid.Parse(tokenDto.UserId) && r.CurrentState == 0);

            foreach (var dbToken in allTokens)
            {
               _repo.ChangeStatus(dbToken.Id,Guid.Parse(tokenDto.UserId));
            }

            var newToken = _mapper.Map<RefreshTokenDto, TbRefreshToken>(tokenDto);
            newToken.CreatedBy = _userService.GetLoggedInUser();
            _repo.Insert(newToken);         
            return true;


        }

        public bool IsExpireToken(string token)
        {
            var storedToken = _refreshTokensRetrival.GetByToken(token);

            if (storedToken == null || storedToken.CurrentState == 1 || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }


    }


}
