using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task SignUpAsync(AccountForSignUpDto account);
        Task<(string, RefreshTokenDto)> SignInAsync(AccountForSignInDto account);
        Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken);
        Task<bool> DeleteAccountAsync(string userName);
        Task<string> SignInFirebaseAsync(AccountForSignInDto account);
        Task SignUpFirebaseAsync(AccountForSignUpDto account);
    }
}
