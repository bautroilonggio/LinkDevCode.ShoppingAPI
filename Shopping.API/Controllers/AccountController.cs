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
            var token = await _userService.SignInAsync(user);
            if (token == null)
            {
                return Unauthorized(new ResponseAPI
                {
                    Status= false,
                    Message = "Sign In failed!"
                });
            }

            return Ok(new ResponseAPI
            {
                Status = true,
                Message = "Sign In success",
                Data = token
            });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult> RefreshTokenAsync(RefreshTokenDto refreshToken)
        {
            try
            {
                var accessToken = await _userService.RefreshTokenAsync(refreshToken);

                return Ok(new ResponseAPI()
                {
                    Status = true,
                    Message = "Created new Access Token",
                    Data = accessToken
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
    }
}
