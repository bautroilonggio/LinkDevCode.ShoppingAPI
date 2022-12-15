using Microsoft.EntityFrameworkCore;
using Identity.DataAccess.DbContexts;
using Identity.DataAccess.Entities;
using System.Linq.Expressions;

namespace Identity.DataAccess.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private readonly IdentityContext _context;
        public AccountRepository(IdentityContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }
    }
}
