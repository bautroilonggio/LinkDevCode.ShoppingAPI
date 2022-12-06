using AutoMapper;
using Firebase.Auth;
using FirebaseAdmin.Auth;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Microsoft.IdentityModel.Tokens;
using Shopping.API.DataAccess;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shopping.API.BusinessLogic.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public AccountService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SignUpAsync(AccountForSignUpDto account)
        {
            if(await _unitOfWork.AccountRepository.IsExistAsync(a => a.UserName == account.UserName))
            {
                throw new Exception("UserName da ton tai!");
            }

            if (await _unitOfWork.AccountRepository.IsExistAsync(a => a.Email == account.Email))
            {
                throw new Exception("Email nay da duoc su dung!");
            }

            if (await _unitOfWork.AccountRepository.IsExistAsync(a => a.Phone == account.Phone))
            {
                throw new Exception("So dien thoai da duoc su dung!");
            }

            var accountEntity = _mapper.Map<Account>(account);

            _unitOfWork.AccountRepository.Add(accountEntity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<(string, RefreshTokenDto)> SignInAsync(AccountForSignInDto account)
        {
            var accountEntity = await _unitOfWork.AccountRepository.GetSingleConditionsAsync(
                                   a => (a.UserName == account.UserName || a.Email == account.UserName) 
                                        && a.Password == account.Password);

            if(accountEntity == null)
            {
                throw new Exception("Incorrect username or password!");
            }

            var accessToken = GenerateAccessToken(accountEntity);

            var refreshToken = GenerateRefreshToken();

            // Cập nhật refresh token mới
            accountEntity.RefreshToken = refreshToken.RefreshToken;
            accountEntity.RefreshTokenCreatedAt = refreshToken.RefreshTokenCreatedAt;
            accountEntity.RefreshTokenExpries = refreshToken.RefreshTokenExpries;

            await _unitOfWork.CommitAsync();

            return (accessToken, refreshToken);
        }

        public async Task SignUpFirebaseAsync(AccountForSignUpDto account)
        {
            var auth = new FirebaseAuthProvider(
                new FirebaseConfig(_configuration["Authentication:Firebase:WebAPIKey"]));

            await auth.CreateUserWithEmailAndPasswordAsync(
                account.Email, account.Password, account.LastName + account.FirstName, true);
        }

        public async Task<object> SignInFirebaseAsync(AccountForSignInDto account)
        {
            var auth = new FirebaseAuthProvider(
                new FirebaseConfig(_configuration["Authentication:Firebase:WebAPIKey"]));

            var t = await auth.SignInWithEmailAndPasswordAsync(account.UserName, account.Password);

            if(t == null)
            {
                throw new Exception("Email or password is invalid!");
            }

            var data = new
            {
                User = t.User,
                AccessToken = t.FirebaseToken,
                RefreshToken = t.RefreshToken,
                ExpiresIn = t.ExpiresIn
            };

            return data;
        }

        //public async Task<object> SignInGoogleAsync(AccountForSignInDto account)
        //{
        //    var auth = new FirebaseAuthProvider(
        //        new FirebaseConfig(_configuration["Authentication:Firebase:WebAPIKey"]));

            
        //}


        public async Task<bool> SignOutAsync(string userName)
        {
            var account = await _unitOfWork.AccountRepository
                .GetSingleConditionsAsync(a => a.UserName == userName);

            if(account == null)
            {
                return false;
            }

            // Xóa refresh token
            account.RefreshToken = "";

            await _unitOfWork.CommitAsync();

            return true;
        }

        public async Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken)
        {
            var accountEntity = await _unitOfWork.AccountRepository
                .GetSingleConditionsAsync(a => a.RefreshToken == refreshToken);

            if(accountEntity == null)
            {
                throw new Exception("Invalid Refresh Token!");
            }
            else if(accountEntity.RefreshTokenExpries < DateTime.Now)
            {
                throw new Exception("Refresh Token expired! Please sign in again!");
            }

            var newAccessToken = GenerateAccessToken(accountEntity);

            var newRefreshToken = GenerateRefreshToken();

            // Cập nhật refresh token mới
            accountEntity.RefreshToken = newRefreshToken.RefreshToken;
            accountEntity.RefreshTokenCreatedAt = newRefreshToken.RefreshTokenCreatedAt;
            accountEntity.RefreshTokenExpries = newRefreshToken.RefreshTokenExpries;

            await _unitOfWork.CommitAsync();

            return (newAccessToken, newRefreshToken);
        }

        public async Task<bool> DeleteAccountAsync(string userName)
        {
            var accountEntity = await _unitOfWork.AccountRepository.GetSingleConditionsAsync(
                                   a => a.UserName == userName);
            if(accountEntity == null)
            {
                return false;
            }

            _unitOfWork.AccountRepository.Delete(accountEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public string GenerateAccessToken(Account account)
        {
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", account.UserName));
            claimsForToken.Add(new Claim("given_name", account.FirstName));
            claimsForToken.Add(new Claim("family_name", account.LastName));
            claimsForToken.Add(new Claim("email", account.Email));
            claimsForToken.Add(new Claim("role", account.Role));

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:Project:SecretForKey"]));

            var signingCredetials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Authentication:Project:Issuer"],
                audience: _configuration["Authentication:Project:Audience"],
                claims: claimsForToken,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signingCredetials);

            var jwtToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return jwtToReturn;
        }

        public RefreshTokenDto GenerateRefreshToken()
        {
            var refreshToken = new RefreshTokenDto()
            {
                RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                RefreshTokenCreatedAt = DateTime.Now,
                RefreshTokenExpries = DateTime.Now.AddDays(1)
            };

            return refreshToken;
        }
    }
}
