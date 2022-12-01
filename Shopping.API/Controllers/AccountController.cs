using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.BusinessLogic.Services;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Models;

namespace Shopping.API.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService ??
                throw new ArgumentNullException(nameof(userService));
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("signup")]
        public async Task<ActionResult> SignUpAsync(UserForSignUpDto user)
        {
            try
            {
                await _userService.SignUpAsync(user);
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
        public async Task<ActionResult> SignInAsync(UserForSignInDto user)
        {
            var (accessToken, refreshToken) = await _userService.SignInAsync(user);
            if (accessToken == null || refreshToken == null)
            {
                return Unauthorized(new ResponseAPI
                {
                    Status= false,
                    Message = "Sign In failed!"
                });
            }

            // Lưu vào cookie
            SetRefreshTokenTnCookie(refreshToken);

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

        //[HttpPost("logout")]
        //public ActionResult LogoutAsync()
        //{
        //    Response.Cookies.Delete("refreshToken");

        //    if(Request.Cookies["refreshToken"] != null)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok();
        //}

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshTokenAsync()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var (newAccessToken, newRefreshToken) = await _userService.RefreshTokenAsync(refreshToken);

                // Lưu vào cookie
                SetRefreshTokenTnCookie(newRefreshToken);

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
            if(!await _userService.DeleteUserAsync(userName))
            {
                return NotFound();
            }

            return NoContent();
        }


        public void SetRefreshTokenTnCookie(RefreshTokenDto refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.RefreshTokenExpries
            };
            Response.Cookies.Append("refreshToken", refreshToken.RefreshToken, cookieOptions);
        }
    }
}
