using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ShoppingContext context) : base(context)
        {
        }
    }
}
