using AutoMapper;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using Shopping.API.DataAccess;
using Shopping.API.DataAccess.Entities;
using Shopping.API.DataAccess.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Shopping.API.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
        }

        public async Task SignUpAsync(UserForSignUpDto user)
        {
            if(await _unitOfWork.UserRepository.IsExistAsync(u => u.UserName == user.UserName))
            {
                throw new Exception("UserName da ton tai!");
            }

            if (await _unitOfWork.UserRepository.IsExistAsync(u => u.Email == user.Email))
            {
                throw new Exception("Email nay da duoc su dung!");
            }

            if (await _unitOfWork.UserRepository.IsExistAsync(u => u.Phone == user.Phone))
            {
                throw new Exception("So dien thoai da duoc su dung!");
            }

            var userEntity = _mapper.Map<User>(user);

            _unitOfWork.UserRepository.Add(userEntity);

            await _unitOfWork.CommitAsync();
        }

        public async Task<(string, RefreshTokenDto)> SignInAsync(UserForSignInDto user)
        {
            var userEntity = await _unitOfWork.UserRepository.GetSingleConditionsAsync(
                                   u => (u.UserName == user.UserName || u.Email == user.UserName) 
                                        && u.Password == user.Password);

            if(userEntity == null)
            {
                throw new Exception("Sign In failed");
            }

            var accessToken = GenerateAccessToken(userEntity);

            var refreshToken = GenerateRefreshToken();

            // Cập nhật refresh token mới
            userEntity.RefreshToken = refreshToken.RefreshToken;
            userEntity.RefreshTokenCreatedAt = refreshToken.RefreshTokenCreatedAt;
            userEntity.RefreshTokenExpries = refreshToken.RefreshTokenExpries;

            await _unitOfWork.CommitAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string, RefreshTokenDto)> RefreshTokenAsync(string refreshToken)
        {
            var userEntity = await _unitOfWork.UserRepository
                .GetSingleConditionsAsync(u => u.RefreshToken == refreshToken);

            if(userEntity == null)
            {
                throw new Exception("Invalid Refresh Token!");
            }
            else if(userEntity.RefreshTokenExpries < DateTime.Now)
            {
                throw new Exception("Refresh Token expired! Please sign in again!");
            }

            var newAccessToken = GenerateAccessToken(userEntity);

            var newRefreshToken = GenerateRefreshToken();

            // Cập nhật refresh token mới
            userEntity.RefreshToken = newRefreshToken.RefreshToken;
            userEntity.RefreshTokenCreatedAt = newRefreshToken.RefreshTokenCreatedAt;
            userEntity.RefreshTokenExpries = newRefreshToken.RefreshTokenExpries;

            await _unitOfWork.CommitAsync();

            return (newAccessToken, newRefreshToken);
        }

        public async Task<bool> DeleteUserAsync(string userName)
        {
            var userEntity = await _unitOfWork.UserRepository.GetSingleConditionsAsync(
                                   u => u.UserName == userName);
            if(userEntity == null)
            {
                return false;
            }

            _unitOfWork.UserRepository.Delete(userEntity);

            await _unitOfWork.CommitAsync();

            return true;
        }

        public string GenerateAccessToken(User user)
        {
            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserName));
            claimsForToken.Add(new Claim("given_name", user.FirstName));
            claimsForToken.Add(new Claim("family_name", user.LastName));
            claimsForToken.Add(new Claim("email", user.Email));
            claimsForToken.Add(new Claim("role", user.Role));

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]));

            var signingCredetials = new SigningCredentials(
                securityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Authentication:Issuer"],
                audience: _configuration["Authentication:Audience"],
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
