using Microsoft.EntityFrameworkCore;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;
using System.Linq.Expressions;

namespace Shopping.API.DataAccess.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        private readonly ShoppingContext _context;
        public AccountRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<Account?> GetUserIncludeReviewsAsync(Expression<Func<Account, bool>> where)
        {
            return await _context.Accounts.Include(u => u.Reviews).Where(where).FirstOrDefaultAsync();
        }

        public async Task<Account?> GetUserIncludeCartsAsync(Expression<Func<Account, bool>> where)
        {
            return await _context.Accounts.Include(u => u.Carts).Where(where).FirstOrDefaultAsync();
        }

        public async Task<Account?> GetUserIncludeOrdersAsync(Expression<Func<Account, bool>> where)
        {
            return await _context.Accounts.Include(u => u.Orders).Where(where).FirstOrDefaultAsync();
        }
    }
}
