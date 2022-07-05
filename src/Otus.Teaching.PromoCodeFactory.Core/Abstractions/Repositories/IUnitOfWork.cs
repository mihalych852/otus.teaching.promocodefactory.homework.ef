using System.Threading;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories
{
    public interface IUnitOfWork
    {
        public Task SaveAsync(CancellationToken cancellationToken = default);
    }
}