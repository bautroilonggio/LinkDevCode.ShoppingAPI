using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Shopping.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _accountService = accountService ??
                throw new ArgumentNullException(nameof(accountService));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync(AccountForSignUpDto account)
        {
            try
            {
                var userNameCurrent = User.FindFirstValue(ClaimTypes.NameIdentifier);

                await _accountService.SignUpAsync(account);

                _logger.LogInformation($"Tai khoan {account.UserName} da duoc them boi ADMIN {userNameCurrent}");

                return Created("Success", new ResponseAPI
                {
                    Status = true,
                    Message = "Sign Up success"
                });
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Them moi tai khoan that bai do {e.Message}");

                return BadRequest(new ResponseAPI
                {
                    Status = false,
                    Message = $"Sign Up failed - {e.Message}"
                });
            }
        }

        [HttpPost("signin")]
        public async Task<ActionResult> SignInAsync(AccountForSignInDto account)
        {
            try
            {
                var (accessToken, refreshToken) = await _accountService.SignInAsync(account);
                if (accessToken == null || refreshToken == null)
                {
                    _logger.LogInformation($"Dang nhap vao tai khoan {account.UserName} khong thanh cong do khong tao duoc token");

                    return Unauthorized(new ResponseAPI
                    {
                        Status = false,
                        Message = "Sign In failed!"
                    });
                }

                _logger.LogInformation($"Tai khoan {account.UserName} da dang nhap thanh cong");

                // Lưu vào cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshToken.RefreshTokenExpries
                };
                Response.Cookies.Append("refreshToken", refreshToken.RefreshToken, cookieOptions);

                return Ok(new ResponseAPI
                {
                    Status = true,
                    Message = "Sign In success",
                    Data = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken.RefreshToken,
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Dang nhap that bai vao tai khoan {account.UserName} do {e.Message}");

                return Unauthorized(e.Message);
            }
        }

        [HttpPost("signin-firebase")]
        public async Task<ActionResult> SignInFirebaseAsync(AccountForSignInDto account)
        {
            var token = await _accountService.SignInFirebaseAsync(account);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(token);
        }

        [HttpPost("signup-firebase")]
        public async Task<ActionResult> SignUpFirebaseAsync(AccountForSignUpDto account)
        {
            await _accountService.SignUpFirebaseAsync(account);
            return Ok();
        }

        //[HttpPost("signin-google")]
        //public async Task<ActionResult<string>> SignInGoogleAsync()
        //{
        //    var accessToken = await HttpContext.GetTokenAsync(GoogleDefaults.AuthenticationScheme, "access_token");

        //    return Ok(accessToken);
        //}

        [HttpPost("logout")]
        public ActionResult LogoutAsync()
        {
            Response.Cookies.Delete("refreshToken");

            if (Request.Cookies["refreshToken"] != null)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var (newAccessToken, newRefreshToken) = await _accountService.RefreshTokenAsync(refreshToken);

                _logger.LogInformation("Da tao moi thanh cong Access Token va Refresh Token");

                // Lưu vào cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = newRefreshToken.RefreshTokenExpries
                };
                Response.Cookies.Append("refreshToken", newRefreshToken.RefreshToken, cookieOptions);

                return Ok(new ResponseAPI()
                {
                    Status = true,
                    Message = "Created new Token",
                    Data = new
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken.RefreshToken
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Tao moi token that bai do {e.Message}");

                return Unauthorized(new ResponseAPI()
                {
                    Status = false,
                    Message = e.Message
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{userName}")]
        public async Task<ActionResult> DeleteAccountAsync(string userName)
        {
            if(!await _accountService.DeleteAccountAsync(userName))
            {
                _logger.LogInformation($"Khong xoa duoc tai khoan {userName} vi khong ton tai tai khoan nay");

                return NotFound();
            }

            var userNameCurrent = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _logger.LogInformation($"Tai khoan {userName} da bi xoa boi ADMIN {userNameCurrent}");

            return NoContent();
        }
    }
}
