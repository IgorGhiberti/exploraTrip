using System.Threading.Tasks;
using Domain.Common;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //TODO Implementar lógica para salvar no banco de dados
            await Task.CompletedTask;
            return 0;
        }
    }
}