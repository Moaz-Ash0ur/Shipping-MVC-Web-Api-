using AutoMapper;
using BL.Contracts;
using BL.DTOs;
using DAL.Contracts;
using Domains;

namespace BL.Services
{
    public class RefreshTokenRetrival : IRefreshTokensRetrival
    {

        private IGenericRepository<TbRefreshToken> _repo;
        private IMapper _mapper;

        public RefreshTokenRetrival(IGenericRepository<TbRefreshToken> repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public RefreshTokenDto GetByToken(string token)
        {
            var myToken = _repo.GetFirst(r => r.Token == token);
            return _mapper.Map<TbRefreshToken, RefreshTokenDto>(myToken);
        }


    }


}
