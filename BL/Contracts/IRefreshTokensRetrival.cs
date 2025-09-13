using BL.DTOs;

namespace BL.Contracts
{
    public interface IRefreshTokensRetrival
    {
        public RefreshTokenDto GetByToken(string token);
    }




}
