using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;
using System.Net.WebSockets;

namespace Shopping.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService ??
                throw new ArgumentNullException(nameof(accountService));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync(AccountForSignUpDto user)
        {
            try
            {
                await _accountService.SignUpAsync(user);
                return Created("Success", new ResponseAPI
                {
                    Status = true,
                    Message = "Sign Up success"
                });
            }
            catch (Exception e)
            {
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
                    return Unauthorized(new ResponseAPI
                    {
                        Status = false,
                        Message = "Sign In failed!"
                    });
                }

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
                return NotFound();
            }

            return NoContent();
        }
    }
}
