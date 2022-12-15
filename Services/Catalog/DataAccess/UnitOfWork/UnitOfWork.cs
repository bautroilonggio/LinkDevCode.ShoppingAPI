using Catalog.DataAccess.DbContexts;
using Catalog.DataAccess.Repositories;

namespace Catalog.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ProductContext _context;


        private IProductRepository? _productRepository;
        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }
                return _productRepository;
            }
        }

        public UnitOfWork(ProductContext context)
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
