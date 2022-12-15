using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Ordering.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContext)
        {
            _httpContextAccessor = httpContext ??
                throw new ArgumentNullException(nameof(httpContext));
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
