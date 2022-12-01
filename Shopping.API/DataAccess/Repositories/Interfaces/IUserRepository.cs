using Shopping.API.DataAccess.Entities;
using System.Linq.Expressions;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetUserIncludeReviewsAsync(Expression<Func<User, bool>> where);
        Task<User?> GetUserIncludeCartsAsync(Expression<Func<User, bool>> where);
        Task<User?> GetUserIncludeOrdersAsync(Expression<Func<User, bool>> where);
    }
}
