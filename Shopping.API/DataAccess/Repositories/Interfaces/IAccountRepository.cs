using Shopping.API.DataAccess.Entities;
using System.Linq.Expressions;

namespace Shopping.API.DataAccess.Repositories
{
    public interface IAccountRepository : IRepositoryBase<Account>
    {
        Task<Account?> GetUserIncludeReviewsAsync(Expression<Func<Account, bool>> where);
        Task<Account?> GetUserIncludeCartsAsync(Expression<Func<Account, bool>> where);
        Task<Account?> GetUserIncludeOrdersAsync(Expression<Func<Account, bool>> where);
        Task<Account?> GetUserIncludeAllAsync(Expression<Func<Account, bool>> where);
    }
}
