using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return 0;
        }
    }
}