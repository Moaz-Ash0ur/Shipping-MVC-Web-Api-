using AutoMapper;
using BL.DTOs;
using BL.Services;
using DAL.Contracts;
using Domains;

namespace BL.Contracts
{
    public interface IRefreshTokens : IBaseService<TbRefreshToken, RefreshTokenDto>
    {
        public bool Refresh(RefreshTokenDto tokenDto);
        public bool IsExpireToken(string token);

    }


   



}
