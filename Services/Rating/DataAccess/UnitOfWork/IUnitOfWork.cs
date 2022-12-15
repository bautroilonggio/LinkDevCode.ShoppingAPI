using Rating.DataAccess.Repositories;

namespace Rating.DataAccess
{
    public interface IUnitOfWork
    {
        IReviewRepository ReviewRepository { get; }
        Task CommitAsync();
    }
}