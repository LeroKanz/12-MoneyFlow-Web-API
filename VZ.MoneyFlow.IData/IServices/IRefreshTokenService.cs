using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using VZ.MoneyFlow.Entities.DbSet;
using VZ.MoneyFlow.Models.Models.Dtos.Requests;
using VZ.MoneyFlow.Models.Models.Dtos.Responses;

namespace VZ.MoneyFlow.IData.IServices
{
    public interface IRefreshTokenService
    {
        Task<ResponseAuthResultDto> VerifyAndGenerateToken(RequestTokenDto requestTokenDto);
        Task<ResponseAuthResultDto> GenerateJwtToken(AppUser appUser);
    }
}
