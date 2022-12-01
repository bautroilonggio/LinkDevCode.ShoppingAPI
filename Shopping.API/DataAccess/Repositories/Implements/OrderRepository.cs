using Microsoft.EntityFrameworkCore;
using Shopping.API.Commons;
using Shopping.API.DataAccess.DbContexts;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        private readonly ShoppingContext _context;

        public OrderRepository(ShoppingContext context) : base(context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task<(IEnumerable<Order>, PaginationMetadata)> GetAllAsync(
            int userId, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            var colection = _context.Orders as IQueryable<Order>;

            colection = colection.Where(o => o.UserId == userId)
                                 .Include(o => o.OrderDetails)
                                 .ThenInclude(o => o.Product);

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

        public void Add(User user, Order order)
        {
            user.Orders.Add(order);
        }

        public void Delete(User user, Order order)
        {
            Delete(order);
            user.Orders.Remove(order);
        }
    }
}
