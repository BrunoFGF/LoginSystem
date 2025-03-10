using LG.Application.Commons.Bases;
using LG.Application.Dtos.Account.Request;
using LG.Application.Dtos.Account.Response;

namespace LG.Application.Interfaces
{
    public interface IAccountApplication
    {
        Task<AccountResponseDto> GetUserAccountAsync();
        Task<BaseResponse<bool>> UpdateAccountAsync(AccountRequestDto request);
    }
}
