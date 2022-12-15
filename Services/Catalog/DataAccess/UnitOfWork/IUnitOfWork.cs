using Catalog.DataAccess.Repositories;

namespace Catalog.DataAccess
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }
        Task CommitAsync();
    }
}