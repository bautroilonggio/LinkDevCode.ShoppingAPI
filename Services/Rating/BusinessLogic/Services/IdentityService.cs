using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Rating.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public string GetUserIdentity()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.Request.Headers["UserId"].ToString();
            }
            return result;
        }
    }
}
