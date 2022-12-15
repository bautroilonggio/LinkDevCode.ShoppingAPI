using Ordering.DataAccess.DbContexts;
using Ordering.DataAccess.Repositories;

namespace Ordering.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly OrderingContext _context;

        private IOrderRepository? _orderRepository;
        public IOrderRepository OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }
                return _orderRepository;
            }
        }


        private IOrderDetailRepository? _orderDetailRepository;
        public IOrderDetailRepository OrderDetailRepository
        {
            get
            {
                if (_orderDetailRepository == null)
                {
                    _orderDetailRepository = new OrderDetailRepository(_context);
                }
                return _orderDetailRepository;
            }
        }


        public UnitOfWork(OrderingContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
