using Shopping.API.DataAccess.Models;

namespace Shopping.API.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task SignUpAsync(AccountForSignUpDto account);
        Task<(string, RefreshTokenDto)> SignInAsync(AccountForSignInDto account);
        Task<bool> SignOutAsync(string userName);
        Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken);
        Task<bool> DeleteAccountAsync(string userName);
        Task<object> SignInWithFirebaseAsync(AccountForSignInDto account);
        Task SignUpFirebaseAsync(AccountForSignUpDto account);

        Task<object> SignInWithGoogleAsync(string idToken);
    }
}
