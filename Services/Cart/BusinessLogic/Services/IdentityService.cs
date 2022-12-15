using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cart.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetUserIdentity()
        {
            var result = string.Empty;
            if(_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.Request.Headers["UserId"].ToString();
            }
            return result;
        }
    }
}
