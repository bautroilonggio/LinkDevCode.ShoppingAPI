using Microsoft.EntityFrameworkCore;
using Ordering.Commons;
using Ordering.DataAccess.DbContexts;
using Ordering.DataAccess.Entities;
using System.Linq.Expressions;

namespace Ordering.DataAccess.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly OrderingContext _context;

        public OrderRepository(OrderingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Order>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Orders as IQueryable<Order>;

            colection = colection.Where(o => o.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                colection = colection.Where(p => (p.ShipName != null && p.ShipName.Contains(searchQuery))
                                            || (p.ShipAddress != null && p.ShipAddress.Contains(searchQuery))
                                            || (p.ShipPhone != null && p.ShipPhone.Contains(searchQuery))
                                            || (p.Status != null && p.Status.Contains(searchQuery)));
            }

            var totalItemCount = await colection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await colection.Skip(pageSize * (pageNumber - 1))  // Bỏ qua nội dung các trang trước đó
                                                    .Take(pageSize)                     // Lấy kích thước trang được yêu cầu hiện tại
                                                    .ToListAsync();                     // Tương tác với CSDL

            return (collectionToReturn, paginationMetadata);
        }

        public async Task<Order?> GetOrderAsync(Expression<Func<Order, bool>> where)
        {
            return await _context.Orders.Include(o => o.OrderDetails)
                                        .Where(where).FirstOrDefaultAsync();
        }
    }
}
