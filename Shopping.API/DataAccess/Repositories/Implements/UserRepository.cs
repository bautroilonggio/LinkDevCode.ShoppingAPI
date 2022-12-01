using Microsoft.EntityFrameworkCore;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;
using System.Linq.Expressions;

namespace Shopping.API.DataAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ShoppingContext _context;
        public UserRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> GetUserIncludeReviewsAsync(Expression<Func<User, bool>> where)
        {
            return await _context.Users.Include(u => u.Reviews).Where(where).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserIncludeCartsAsync(Expression<Func<User, bool>> where)
        {
            return await _context.Users.Include(u => u.Carts).Where(where).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserIncludeOrdersAsync(Expression<Func<User, bool>> where)
        {
            return await _context.Users.Include(u => u.Orders).Where(where).FirstOrDefaultAsync();
        }
    }
}
