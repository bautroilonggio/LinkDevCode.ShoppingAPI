using Ordering.DataAccess.DbContexts;
using Ordering.DataAccess.Entities;

namespace Ordering.DataAccess.Repositories
{
    public class OrderDetailRepository : RepositoryBase<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(OrderingContext context) : base(context)
        {
        }
    }
}
