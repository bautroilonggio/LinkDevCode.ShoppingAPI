using Rating.DataAccess.DbContexts;
using Rating.DataAccess.Repositories;

namespace Rating.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ReviewContext _context;

        private IReviewRepository? _reviewRepository;
        public IReviewRepository ReviewRepository
        {
            get
            {
                if (_reviewRepository == null)
                {
                    _reviewRepository = new ReviewRepository(_context);
                }
                return _reviewRepository;
            }
        }

        public UnitOfWork(ReviewContext context)
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
