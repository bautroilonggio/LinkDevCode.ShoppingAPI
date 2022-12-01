using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IUserService
    {
        Task SignUpAsync(UserForSignUpDto user);
        Task<(string, RefreshTokenDto)> SignInAsync(UserForSignInDto user);
        Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken);
        Task<bool> DeleteUserAsync(string userName);
    }
}
