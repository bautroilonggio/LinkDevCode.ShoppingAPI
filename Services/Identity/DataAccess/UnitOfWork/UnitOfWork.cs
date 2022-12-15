using Identity.DataAccess.DbContexts;
using Identity.DataAccess.Repositories;

namespace Identity.DataAccess
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IdentityContext _context;

        private IAccountRepository? _accountRepository;
        public IAccountRepository AccountRepository
        {
            get
            {
                if (_accountRepository == null)
                {
                    _accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }

        public UnitOfWork(IdentityContext context)
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
