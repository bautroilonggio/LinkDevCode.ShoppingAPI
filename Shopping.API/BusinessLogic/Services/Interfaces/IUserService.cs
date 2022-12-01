using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IUserService
    {
        Task SignUpAsync(UserForSignUpDto user);
        Task<TokenDto?> SignInAsync(UserForSignInDto user);
        Task<string> RefreshTokenAsync(RefreshTokenDto refreshToken);
        Task<bool> DeleteUserAsync(string userName);
    }
}
