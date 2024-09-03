using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories.Interfaces
{
    public interface IContaCorrenteRepository
    {
        Task<ContaCorrente> GetByIdAsync(string id);
    }
}
