
using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IIdempotenciaRepository
    {
        Task AddAsync(Idempotencia novoRegistroIdempotencia);
        Task<Idempotencia> GetByChaveAsync(Guid requestId);
    }
}
