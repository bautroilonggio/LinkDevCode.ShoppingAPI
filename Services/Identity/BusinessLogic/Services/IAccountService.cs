using Identity.DataAccess.Models;

namespace Identity.BusinessLogic.Services
{
    public interface IAccountService
    {
        Task SignUpAsync(AccountForSignUpDto account);
        Task<(string, RefreshTokenDto)> SignInAsync(AccountForSignInDto account);
        Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken);
        Task<bool> DeleteAccountAsync(string userName);
        Task<object> SignInWithFirebaseAsync(AccountForSignInWithFirebaseDto account);
        Task SignUpWithFirebaseAsync(AccountForSignUpWithFirebaseDto account);
        Task<object> SignInWithGoogleAsync(string accessToken);
    }
}
