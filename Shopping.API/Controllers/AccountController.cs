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
    /// <summary>
    /// Controller provides actions for account management
    /// </summary>
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        private readonly IAccountService _accountService;

        /// <summary>
        /// Contructor of account controller
        /// </summary>
        /// <param name="logger">Log actions</param>
        /// <param name="accountService">Provide methods</param>
        /// <exception cref="ArgumentNullException">Check for null</exception>
        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _accountService = accountService ??
                throw new ArgumentNullException(nameof(accountService));
        }

        /// <summary>
        /// Create a new account by ADMIN account
        /// </summary>
        /// <param name="account">Information new account</param>
        /// <returns>ActionResult</returns>
        [Authorize(Roles = "ADMIN")]
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Sign in account
        /// </summary>
        /// <param name="account">Information sign in of account</param>
        /// <returns>ActionResult</returns>
        [HttpPost("signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SignInAsync(AccountForSignInDto account)
        {
            try
            {
                var (accessToken, refreshToken) = await _accountService.SignInAsync(account);

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

        /// <summary>
        /// Sign in with firebase account
        /// </summary>
        /// <param name="account">Information of firebase account</param>
        /// <returns>ActionResult</returns>
        [HttpPost("signin-firebase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> SignInFirebaseAsync(AccountForSignInDto account)
        {
            try
            {
                var data = await _accountService.SignInFirebaseAsync(account);

                return Ok(new ResponseAPI
                {
                    Status = true,
                    Message = "Sign In success!",
                    Data = data
                });
            }
            catch(Exception e)
            {
                return Unauthorized(e.Message);
            }
        }

        /// <summary>
        /// Create a new firebase account
        /// </summary>
        /// <param name="account">Information new firebase account</param>
        /// <returns>An ActionResult</returns>
        [HttpPost("signup-firebase")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> SignUpFirebaseAsync(AccountForSignUpDto account)
        {
            await _accountService.SignUpFirebaseAsync(account);
            return Ok();
        }

        /// <summary>
        /// Logout account, delete cookie
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult LogoutAsync()
        {
            Response.Cookies.Delete("refreshToken");

            if (Request.Cookies["refreshToken"] != null)
            {
                return BadRequest();
            }

            return Ok();
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

        /// <summary>
        /// Delete an account by ADMIN account
        /// </summary>
        /// <param name="userName">The username of account that with be deleted</param>
        /// <returns></returns>
        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{userName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
