using Cart.DataAccess.DbContexts;
using Cart.DataAccess.Repositories;

namespace Cart.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CartContext _context;

        private ICartRepository? _cartRepository;
        public ICartRepository CartRepository
        {
            get
            {
                if (_cartRepository == null)
                {
                    _cartRepository = new CartRepository(_context);
                }
                return _cartRepository;
            }
        }

        public UnitOfWork(CartContext context)
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
